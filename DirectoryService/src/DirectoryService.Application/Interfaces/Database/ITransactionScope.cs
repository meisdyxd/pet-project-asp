using CSharpFunctionalExtensions;
using DirectoryService.Contracts.Errors;

namespace DirectoryService.Application.Interfaces.Database;

public interface ITransactionScope : IDisposable
{
    UnitResult<ErrorList> Commit();
    UnitResult<ErrorList> Rollback();
}