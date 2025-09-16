using CSharpFunctionalExtensions;
using DirectoryService.Contracts;

namespace DirectoryService.Application.Interfaces;

public interface ITransactionScope : IDisposable
{
    UnitResult<ErrorList> Commit();
    UnitResult<ErrorList> Rollback();
}