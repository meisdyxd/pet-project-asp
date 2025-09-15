using DirectoryService.Application.Interfaces.CQRS;
using DirectoryService.Contracts;

namespace DirectoryService.Application.CQRS.Commands.AddLocation;

public class AddLocationCommand : ICommand
{
    public AddLocationCommand(
        string name,
        AddressDto address,
        string timezone)
    {
        Name = name;
        Address = address;
        Timezone = timezone;
    }
    
    public string Name { get; init; }
    public AddressDto Address { get; init; }
    public string Timezone { get; init; }
}