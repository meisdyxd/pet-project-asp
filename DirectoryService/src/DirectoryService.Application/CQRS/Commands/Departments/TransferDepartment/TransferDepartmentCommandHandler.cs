using CSharpFunctionalExtensions;
using DirectoryService.Application.Extensions;
using DirectoryService.Application.Interfaces.CQRS;
using DirectoryService.Application.Interfaces.Database;
using DirectoryService.Application.Interfaces.Database.IRepositories;
using DirectoryService.Contracts.Errors;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace DirectoryService.Application.CQRS.Commands.Departments.TransferDepartment;

public class TransferDepartmentCommandHandler : ICommandHandler<TransferDepartmentCommand>
{
    private readonly ILogger<TransferDepartmentCommandHandler> _logger;
    private readonly IDepartmentsRepository _departmentsRepository;
    private readonly ITransactionManager _transactionManager;
    private readonly IValidator<TransferDepartmentCommand> _validator;

    public TransferDepartmentCommandHandler(
        IDepartmentsRepository departmentsRepository,
        ITransactionManager transactionManager,
        IValidator<TransferDepartmentCommand> validator,
        ILogger<TransferDepartmentCommandHandler> logger)
    {
        _departmentsRepository = departmentsRepository;
        _transactionManager = transactionManager;
        _validator = validator;
        _logger = logger;
    }
    
    public async Task<UnitResult<ErrorList>> Handle(
        TransferDepartmentCommand command, 
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToErrorList();

        //Открываем транзакцию
        var transactionResult = await _transactionManager.BeginTransactionAsync(cancellationToken);
        if (transactionResult.IsFailure)
            return transactionResult.Error;
        using var transaction = transactionResult.Value;

        //Наложить пессимистичную блокировку и получить Department
        var lockDepartmentResult = await _departmentsRepository.GetByIdWithLockAsync(
            command.DepartmentId, 
            transaction.GetTransaction, 
            cancellationToken);
        if (lockDepartmentResult.IsFailure)
        {
            transaction.Rollback();
            return lockDepartmentResult.Error;
        }

        //Проверка на существование Department
        if (lockDepartmentResult.Value is null)
        {
            transaction.Rollback();
            return Errors.Http.NotFound("Department is not found");
        }

        //Проверка на активность Department
        if (lockDepartmentResult.Value.IsActive == false)
        {
            transaction.Rollback();
            return Errors.Http.BadRequestError("Department is inactive");
        }

        //Проверка на то, что Department идет в тот же путь
        if (lockDepartmentResult.Value.ParentId == command.Request.ParentId)
        {
            transaction.Rollback();
            return UnitResult.Success<ErrorList>();
        }
        string? toPath = null;
        if (command.Request.ParentId is not null)
        {
            var parentDepartment = await _departmentsRepository.GetByIdAsync(
                command.Request.ParentId.Value, 
                cancellationToken);

            //Проверка, что существуе родительский Department, если parentId не null
            if (parentDepartment is null)
            {
                transaction.Rollback();
                return Errors.Http.UnprocessableContent("Parent department is not found");
            }

            //Проверка на активность родительского Department
            if (!parentDepartment.IsActive)
            {
                transaction.Rollback();
                return Errors.Http.UnprocessableContent("Parent department is inactive");
            }

            toPath = parentDepartment.Path.Value;

            //Проверить что целевой отдел не ребенок или не сам отдел
            var isTransferableResult = await _departmentsRepository.CheckTransferable(
                command.DepartmentId, 
                command.Request.ParentId, 
                transaction.GetTransaction,
                cancellationToken);
            if (isTransferableResult.IsFailure)
            {
                transaction.Rollback();
                return isTransferableResult.Error;
            }

            if (!isTransferableResult.Value)
            {
                transaction.Rollback();
                return Errors.Http.BadRequestError("Parent department is source department or his child");
            }
        }

        //Обновление путей
        var resultAction = await _departmentsRepository.TransferDepartmentAsync(
            lockDepartmentResult.Value.Path, 
            toPath, 
            transaction.GetTransaction, 
            cancellationToken);

        if (resultAction.IsFailure)
        {
            transaction.Rollback();
            return resultAction.Error;
        }
        await _departmentsRepository.UpdateParentAsync(command.DepartmentId, command.Request.ParentId, cancellationToken);
        await _transactionManager.SaveChangesAsync(cancellationToken);
        transaction.Commit();
        _logger.LogInformation("Department with id - '{fromId}' transfered to - '{toId}'", command.DepartmentId, command.Request.ParentId);
        return UnitResult.Success<ErrorList>();
    }
}