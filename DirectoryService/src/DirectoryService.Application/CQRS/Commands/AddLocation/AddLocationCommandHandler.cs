using CSharpFunctionalExtensions;
using DirectoryService.Application.Interfaces.CQRS;
using DirectoryService.Application.Interfaces.IRepositories;
using DirectoryService.Contracts;
using DirectoryService.Contracts.Extensions;
using DirectoryService.Domain;
using DirectoryService.Domain.ValueObjects.Common;
using DirectoryService.Domain.ValueObjects.Location;
using Microsoft.Extensions.Logging;

namespace DirectoryService.Application.CQRS.Commands.AddLocation;

public class AddLocationCommandHandler : ICommandHandler<AddLocationCommand>
{
    private readonly ILocationsRepository _locationsRepository;
    private readonly ILogger<AddLocationCommandHandler> _logger;
    
    public AddLocationCommandHandler(
        ILocationsRepository locationsRepository, 
        ILogger<AddLocationCommandHandler> logger)
    {
        _locationsRepository = locationsRepository;
        _logger = logger;
    }
    
    public async Task<UnitResult<ErrorList>> Handle(AddLocationCommand command, CancellationToken cancellationToken)
    {
        var name = Name.Create(command.Name);
        if (name.IsFailure)
            return name.Error.ToErrorList();
        
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
            return Errors.InvalidValue.Default("address").ToErrorList();
        
        var timezone = IANATimezone.Create(command.Timezone);
        if (timezone.IsFailure)
            return Errors.InvalidValue.Default("timezone").ToErrorList();
        
        var location = Location.Create(name.Value, address.Value, timezone.Value);
        if (location.IsFailure)
            return Errors.InvalidValue.Default("location").ToErrorList();
        
        await _locationsRepository.AddAsync(location.Value, cancellationToken);
        var result = await _locationsRepository.SaveChangesAsync(cancellationToken);
        if (result.IsFailure)
        {
            _logger.LogError("Error when save location to DB");
            return result.Error.ToErrorList();
        }

        _logger.LogInformation("Location with ID {ID} was successfully added.", location.Value.Id);
        return UnitResult.Success<ErrorList>();
    }
}