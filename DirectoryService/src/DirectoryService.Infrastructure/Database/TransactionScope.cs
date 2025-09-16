using System.Data;
using CSharpFunctionalExtensions;
using DirectoryService.Application.Interfaces;
using DirectoryService.Application.Interfaces.Database;
using DirectoryService.Contracts;
using DirectoryService.Contracts.Errors;
using DirectoryService.Contracts.Extensions;
using Microsoft.Extensions.Logging;

namespace DirectoryService.Infrastructure.Database;

public class TransactionScope : ITransactionScope
{
    private readonly IDbTransaction _transaction;
    private readonly ILogger<TransactionScope> _logger;
    public TransactionScope(
        IDbTransaction transaction, 
        ILogger<TransactionScope> logger)
    {
        _transaction = transaction;
        _logger = logger;
    }

    public UnitResult<ErrorList> Commit()
    {
        try
        {
            _transaction.Commit();
            return UnitResult.Success<ErrorList>();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error committing transaction");
            return Errors.DbErrors.CommitTransaction().ToErrorList();
        }
    }

    public UnitResult<ErrorList> Rollback()
    {
        try
        {
            _transaction.Rollback();
            return UnitResult.Success<ErrorList>();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error committing transaction");
            return Errors.DbErrors.CommitTransaction().ToErrorList();
        }
    }

    public void Dispose()
    {
        _transaction.Dispose();
    }
}