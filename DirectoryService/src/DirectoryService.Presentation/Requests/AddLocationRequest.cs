using DirectoryService.Application.CQRS.Commands.AddLocation;
using DirectoryService.Contracts;

namespace DirectoryService.Presentation.Requests;

public sealed record AddLocationRequest
{
    public string Name { get; init; } = null!;
    public AddressDto Address { get; init; } = null!;
    public string Timezone { get; init; } = null!;

    public AddLocationCommand ToCommand()
        => new AddLocationCommand(Name, Address, Timezone);
}