using CSharpFunctionalExtensions;
using DirectoryService.Application.Interfaces;
using DirectoryService.Application.Interfaces.Database;
using DirectoryService.Contracts;
using DirectoryService.Contracts.Errors;
using DirectoryService.Contracts.Extensions;
using DirectoryService.Infrastructure.Database.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Npgsql;
using System.Net;

namespace DirectoryService.Infrastructure.Database;

public class TransactionManager : ITransactionManager
{
    private readonly DirectoryServiceContext _context;
    private readonly ILogger<TransactionManager> _logger;
    private readonly ILoggerFactory _loggerFactory;

    public TransactionManager(
        DirectoryServiceContext context, 
        ILoggerFactory loggerFactory,
        ILogger<TransactionManager> logger)
    {
        _context = context;
        _loggerFactory = loggerFactory;
        _logger = logger;
    }

    public async Task<Result<ITransactionScope, ErrorList>> BeginTransactionAsync(CancellationToken cancellationToken)
    {
        try
        {
            var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
            var transactionScopeLogger = _loggerFactory.CreateLogger<TransactionScope>();
            var transactionScope = new TransactionScope(transaction.GetDbTransaction(), transactionScopeLogger);
            
            return transactionScope;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error occured during create transaction");
            return Errors.DbErrors.BeginTransaction();
        }
    }

    public async Task<UnitResult<ErrorList>> SaveChangesAsync(CancellationToken cancellationToken)
    {
        try
        {
            await _context.SaveChangesAsync(cancellationToken);
            return UnitResult.Success<ErrorList>();
        }
        catch (DbUpdateException ex) when (ex.InnerException is PostgresException pg && pg.SqlState == "23505")
        {
            _logger.LogError(ex, "Error during save changes: violation of the uniqueness of the index");
            return Errors.DbErrors.WhenSave($"Violdation of the uniqueness of the index", HttpStatusCode.Conflict);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occured during save changes");
            return Errors.DbErrors.WhenSave(ex.Message);
        }
    }
}