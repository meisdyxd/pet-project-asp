using CSharpFunctionalExtensions;
using DirectoryService.Contracts.Errors;

namespace DirectoryService.Application.Interfaces.CQRS;

public interface IQueryHandler<in TQuery, TResponse>
    where TQuery : IQuery<TResponse>
{
    Task<Result<TResponse, ErrorList>> Handle(TQuery query, CancellationToken cancellationToken);
}