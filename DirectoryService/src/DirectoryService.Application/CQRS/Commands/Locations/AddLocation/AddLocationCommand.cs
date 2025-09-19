using DirectoryService.Application.Interfaces.CQRS;
using DirectoryService.Contracts.Requests.LocationRequests;

namespace DirectoryService.Application.CQRS.Commands.Locations.AddLocation;

public sealed record AddLocationCommand(AddLocationRequest request) : ICommand
{
    public AddLocationRequest Request { get; init; } = request;
}