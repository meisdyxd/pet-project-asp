using DirectoryService.Application.Interfaces.CQRS;
using DirectoryService.Contracts.Requests;

namespace DirectoryService.Application.CQRS.Commands.AddPosition;

public class AddPositionCommand(AddPositionRequest request) : ICommand
{
    public AddPositionRequest Request { get; init; } = request;
}