namespace DirectoryService.Contracts;

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

    public string Country { get; }
    public string Region { get; }
    public string City { get; }
    public string Street { get; }
    public string HouseNumber { get; }
    public string? PostalCode { get; }
    public string? District { get; }
    public string? Building { get; }
    public string? Apartment { get; }
}