using System.Net;
using CSharpFunctionalExtensions;
using DirectoryService.Contracts;
using DirectoryService.Contracts.Errors;

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

    public static Result<Address, ErrorList> Create(
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
        var errors = new List<Error>();
        
        if (string.IsNullOrWhiteSpace(country))
            errors.Add(Errors.InvalidValue.Empty(nameof(country)));

        if (string.IsNullOrWhiteSpace(region))
            errors.Add(Errors.InvalidValue.Empty(nameof(region)));

        if (string.IsNullOrWhiteSpace(city))
            errors.Add(Errors.InvalidValue.Empty(nameof(city)));

        if (string.IsNullOrWhiteSpace(street))
            errors.Add(Errors.InvalidValue.Empty(nameof(street)));

        if (string.IsNullOrWhiteSpace(houseNumber))
            errors.Add(Errors.InvalidValue.Empty(nameof(houseNumber)));

        if (errors.Count != 0)
            return new ErrorList(errors, HttpStatusCode.BadRequest);
        
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
