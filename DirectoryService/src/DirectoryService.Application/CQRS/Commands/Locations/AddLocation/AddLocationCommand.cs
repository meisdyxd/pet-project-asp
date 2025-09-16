using DirectoryService.Application.Interfaces.CQRS;
using DirectoryService.Contracts;
using DirectoryService.Contracts.Requests;

namespace DirectoryService.Application.CQRS.Commands.AddLocation;

public class AddLocationCommand(AddLocationRequest request) : ICommand
{
    public AddLocationRequest Request { get; init; } = request;
}