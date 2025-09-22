using DirectoryService.Contracts.Dtos;
using DirectoryService.Contracts.Respones.CommonResponses;

namespace DirectoryService.Contracts.Respones.LocationResponses;

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