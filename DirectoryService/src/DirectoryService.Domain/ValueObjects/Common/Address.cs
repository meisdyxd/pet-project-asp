using CSharpFunctionalExtensions;
using DirectoryService.Contracts;

namespace DirectoryService.Domain.ValueObjects.Common;

public class Address : ValueObject
{
    private Address(
        string country, 
        string region, 
        string city, 
        string street, 
        string houseNumber, 
        string? postalCode, 
        string? district, 
        string? building, 
        string? apartment)
    {
        Country = country;
        Region = region;
        City = city;
        Street = street;
        HouseNumber = houseNumber;
        PostalCode = postalCode;
        District = district;
        Building = building;
        Apartment = apartment;
    }

    /// <summary>
    /// Страна
    /// </summary>
    public string Country { get; }
    /// <summary>
    /// Субъект РФ
    /// </summary>
    public string Region { get; }
    /// <summary>
    /// Город
    /// </summary>
    public string City { get; }
    /// <summary>
    /// Улица
    /// </summary>
    public string Street { get; }
    /// <summary>
    /// Номер дома
    /// </summary>
    public string HouseNumber { get; }
    /// <summary>
    /// Почтовый индекс
    /// </summary>
    public string? PostalCode { get; }
    /// <summary>
    /// Район города
    /// </summary>
    public string? District { get; }
    /// <summary>
    /// Корпус/Строение
    /// </summary>
    public string? Building { get; }
    /// <summary>
    /// Квартира/Офис
    /// </summary>
    public string? Apartment { get; }

    public static Result<Address, Error> Create(
        string country,
        string region,
        string city,
        string street,
        string houseNumber,
        string? postalCode = null,
        string? district = null,
        string? building = null,
        string? apartment = null)
    {
        if (string.IsNullOrWhiteSpace(country))
            return Errors.InvalidValue.Empty(nameof(country));

        if (string.IsNullOrWhiteSpace(region))
            return Errors.InvalidValue.Empty(nameof(region));

        if (string.IsNullOrWhiteSpace(city))
            return Errors.InvalidValue.Empty(nameof(city));

        if (string.IsNullOrWhiteSpace(street))
            return Errors.InvalidValue.Empty(nameof(street));

        if (string.IsNullOrWhiteSpace(houseNumber))
            return Errors.InvalidValue.Empty(nameof(houseNumber));

        return new Address(
            country,
            region, 
            city, 
            street, 
            houseNumber, 
            postalCode, 
            district, 
            building, 
            apartment);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Country;
        yield return Region;
        yield return City;
        yield return Street;
        yield return HouseNumber;
        yield return PostalCode;
        yield return District;
        yield return Building;
        yield return Apartment;
    }
}
