using DirectoryService.Application.CQRS.Commands.Locations.AddLocation;
using DirectoryService.Application.Extensions;
using DirectoryService.Contracts;
using DirectoryService.Domain.ValueObjects.Common;
using FluentValidation;

namespace DirectoryService.Application.Validators.LocationsValidators;

public class AddLocationCommandValidator : AbstractValidator<AddLocationCommand>
{
    public AddLocationCommandValidator()
    {
        RuleFor(r => r.Request.Name)
            .Must(n => 
                n.Length is >= Constants.LocationConstants.MIN_LENGTH_NAME 
                    and <= Constants.LocationConstants.MAX_LENGTH_NAME)
            .WithError(
                "name", 
                $"Length must be in range {Constants.LocationConstants.MIN_LENGTH_NAME} " +
                $"to {Constants.LocationConstants.MAX_LENGTH_NAME}.");

        RuleFor(r => r.Request.Address)
            .MustBeValueObject(a 
                => Address.Create(a.Country, a.Region, a.City, a.Street, a.HouseNumber, a.PostalCode,
                a.District, a.Building, a.Apartment));

        RuleFor(r => r.Request.Timezone)
            .MustBeValueObject(IANATimezone.Create);
    }
}