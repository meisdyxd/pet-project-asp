using CSharpFunctionalExtensions;
using DirectoryService.Application.Interfaces.CQRS;
using DirectoryService.Application.Interfaces.IRepositories;
using DirectoryService.Contracts;
using DirectoryService.Domain;
using DirectoryService.Domain.ValueObjects.Common;
using DirectoryService.Domain.ValueObjects.Location;

namespace DirectoryService.Application.CQRS.Commands.AddLocation;

public class AddLocationCommandHandler : ICommandHandler<AddLocationCommand>
{
    private readonly ILocationsRepository _locationsRepository;

    public AddLocationCommandHandler(ILocationsRepository locationsRepository)
    {
        _locationsRepository = locationsRepository;
    }


    public async Task<UnitResult<Error>> Handle(AddLocationCommand command, CancellationToken cancellationToken)
    {
        var name = Name.Create(command.Name);
        if (name.IsFailure)
            return Errors.InvalidValue.Default("name");
        
        var address = Address.Create(
            command.Address.Country,
            command.Address.Region,
            command.Address.City,
            command.Address.Street,
            command.Address.HouseNumber,
            command.Address.PostalCode,
            command.Address.District,
            command.Address.Building,
            command.Address.Apartment);
        if (address.IsFailure)
            return Errors.InvalidValue.Default("address");
        
        var timezone = IANATimezone.Create(command.Timezone);
        if (timezone.IsFailure)
            return Errors.InvalidValue.Default("timezone");
        
        var location = Location.Create(name.Value, address.Value, timezone.Value);
        if (location.IsFailure)
            return Errors.InvalidValue.Default("location");
        
        await _locationsRepository.AddAsync(location.Value, cancellationToken);
        await _locationsRepository.SaveChangesAsync(cancellationToken);

        return UnitResult.Success<Error>();
    }
}