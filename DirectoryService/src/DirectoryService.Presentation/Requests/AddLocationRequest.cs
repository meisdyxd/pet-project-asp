using DirectoryService.Application.CQRS.Commands.AddLocation;
using DirectoryService.Contracts;

namespace DirectoryService.Presentation.Requests;

public sealed record AddLocationRequest
{
    public string Name { get; init; }
    public AddressDto Address { get; init; }
    public string Timezone { get; init; }
    
    public AddLocationCommand ToCommand()
        => new AddLocationCommand()
        {
            Address = this.Address,
            Timezone = this.Timezone,
            Name = this.Name
        };
}