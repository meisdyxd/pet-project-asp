using DirectoryService.Contracts.Dtos.Locations;
using DirectoryService.Contracts.Responses.CommonResponses;

namespace DirectoryService.Contracts.Responses.LocationResponses;

public sealed record GetLocationsResponse : PaginationResponse
{
    public GetLocationsResponse(
        IReadOnlyList<GetLocationDto> locations,
        int page, 
        int pageSize, 
        long totalItems = 0)
        : base(page, pageSize, totalItems)
    {
        Items = locations;
    }
    public IReadOnlyList<GetLocationDto> Items { get; init; } = [];
}