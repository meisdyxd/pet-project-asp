using CSharpFunctionalExtensions;
using DirectoryService.Contracts.Errors;

namespace DirectoryService.Application.Interfaces.Database;

public interface ITransactionManager
{
    Task<Result<ITransactionScope, ErrorList>> BeginTransactionAsync(CancellationToken cancellationToken);
    Task<UnitResult<ErrorList>> SaveChangesAsync(CancellationToken cancellationToken);
}