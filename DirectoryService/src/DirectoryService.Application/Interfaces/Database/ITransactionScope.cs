using CSharpFunctionalExtensions;
using DirectoryService.Contracts.Errors;
using System.Data;

namespace DirectoryService.Application.Interfaces.Database;

public interface ITransactionScope : IDisposable
{
    IDbTransaction GetTransaction { get; }
    UnitResult<ErrorList> Commit();
    UnitResult<ErrorList> Rollback();
}