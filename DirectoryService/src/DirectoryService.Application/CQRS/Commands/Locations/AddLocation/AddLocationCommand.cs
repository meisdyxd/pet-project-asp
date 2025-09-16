using DirectoryService.Application.Interfaces.CQRS;
using DirectoryService.Contracts.Requests.LocationRequests;

namespace DirectoryService.Application.CQRS.Commands.Locations.AddLocation;

public class AddLocationCommand(AddLocationRequest request) : ICommand
{
    public AddLocationRequest Request { get; init; } = request;
}