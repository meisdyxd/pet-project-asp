using CSharpFunctionalExtensions;
using DirectoryService.Contracts;

namespace DirectoryService.Application.Interfaces.CQRS;

public interface ICommandHandler<in TCommand, TResponse>
    where TCommand : ICommand<TResponse>
{
    Task<Result<TResponse, Error>> Handle(TCommand command, CancellationToken cancellationToken);
}

public interface ICommandHandler<in TCommand>
    where TCommand : ICommand
{
    Task<UnitResult<Error>> Handle(TCommand command, CancellationToken cancellationToken);
}