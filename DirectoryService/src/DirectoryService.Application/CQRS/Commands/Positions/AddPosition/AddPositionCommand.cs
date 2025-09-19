using DirectoryService.Application.Interfaces.CQRS;
using DirectoryService.Contracts.Requests.PositionRequests;

namespace DirectoryService.Application.CQRS.Commands.Positions.AddPosition;

public sealed record AddPositionCommand(AddPositionRequest request) : ICommand
{
    public AddPositionRequest Request { get; init; } = request;
}