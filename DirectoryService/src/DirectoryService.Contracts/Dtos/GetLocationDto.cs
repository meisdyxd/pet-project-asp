namespace DirectoryService.Contracts.Dtos;

public sealed record GetLocationDto
{
    public GetLocationDto(
        Guid id,
        string name,
        string country,
        string region,
        string city, 
        string street,
        string houseNumber,
        string? postalCode,
        string? district,
        string? building,
        string timezone)
    {
        Id = id;
        Name = name;
        Country = country;
        Region = region;
        City = city;
        Street = street;
        HouseNumber = houseNumber;
        PostalCode = postalCode;
        District = district;
        Building = building;
        Timezone = timezone;
    }

    public Guid Id { get; init; }
    public string Name { get; init; }
    public string Timezone { get; init; }
    public string Country { get; init; }
    public string Region { get; init; }
    public string City { get; init; }
    public string Street { get; init; }
    public string HouseNumber { get; init; }
    public string? PostalCode { get; init; }
    public string? District { get; init; }
    public string? Building { get; init; }
}