namespace DirectoryService.Contracts.Dtos;

public sealed record AddressDto
{
    public AddressDto(
        string country,
        string region,
        string city,
        string street,
        string houseNumber, 
        string? postalCode,
        string? district,
        string? building)
    {
        Country = country;
        Region = region;
        City = city;
        Street = street;
        HouseNumber = houseNumber;
        PostalCode = postalCode;
        District = district;
        Building = building;
    }

    public string Country { get; init; }
    public string Region { get; init; }
    public string City { get; init; }
    public string Street { get; init; }
    public string HouseNumber { get; init; }
    public string? PostalCode { get; init; }
    public string? District { get; init; }
    public string? Building { get; init; }
}