using CSharpFunctionalExtensions;
using DirectoryService.Application.Extensions;
using DirectoryService.Application.Interfaces;
using DirectoryService.Application.Interfaces.CQRS;
using DirectoryService.Application.Interfaces.IRepositories;
using DirectoryService.Contracts;
using DirectoryService.Contracts.Extensions;
using DirectoryService.Domain;
using DirectoryService.Domain.ValueObjects.Common;
using DirectoryService.Domain.ValueObjects.Location;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace DirectoryService.Application.CQRS.Commands.AddLocation;

public class AddLocationCommandHandler : ICommandHandler<AddLocationCommand>
{
    private readonly ILocationsRepository _locationsRepository;
    private readonly ITransactionManager _transactionManager;
    private readonly ILogger<AddLocationCommandHandler> _logger;
    private readonly IValidator<AddLocationCommand>  _validator;
    
    public AddLocationCommandHandler(
        ILocationsRepository locationsRepository, 
        ITransactionManager transactionManager,
        IValidator<AddLocationCommand> validator,
        ILogger<AddLocationCommandHandler> logger)
    {
        _locationsRepository = locationsRepository;
        _transactionManager = transactionManager;
        _validator = validator;
        _logger = logger;
    }
    
    public async Task<UnitResult<ErrorList>> Handle(AddLocationCommand command, CancellationToken cancellationToken)
    {
        var resultValidation = await _validator.ValidateAsync(command, cancellationToken);
        if (!resultValidation.IsValid)
            return resultValidation.ToErrorList();
        
        var name = Name.Create(command.Request.Name).Value;
        var addressDto = command.Request.Address;
        var address = Address.Create(
            addressDto.Country,
            addressDto.Region,
            addressDto.City,
            addressDto.Street,
            addressDto.HouseNumber,
            addressDto.PostalCode,
            addressDto.District,
            addressDto.Building,
            addressDto.Apartment).Value;
        var timezone = IANATimezone.Create(command.Request.Timezone).Value;
        
        var location = Location.Create(name, address, timezone);
        if (location.IsFailure) 
            return Errors.InvalidValue.Default("location").ToErrorList();
        
        await _locationsRepository.AddAsync(location.Value, cancellationToken);
        var result = await _transactionManager.SaveChangesAsync(cancellationToken);
        if (result.IsFailure)
            return result.Error;

        _logger.LogInformation("Location with ID '{ID}' was successfully added.", location.Value.Id);
        return UnitResult.Success<ErrorList>();
    }
}