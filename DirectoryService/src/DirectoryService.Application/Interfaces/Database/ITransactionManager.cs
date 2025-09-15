using System.Transactions;
using CSharpFunctionalExtensions;
using DirectoryService.Contracts;

namespace DirectoryService.Application.Interfaces;

public interface ITransactionManager
{
    Task<Result<ITransactionScope, ErrorList>> BeginTransactionAsync(CancellationToken cancellationToken);
    Task<UnitResult<ErrorList>> SaveChangesAsync(CancellationToken cancellationToken);
}