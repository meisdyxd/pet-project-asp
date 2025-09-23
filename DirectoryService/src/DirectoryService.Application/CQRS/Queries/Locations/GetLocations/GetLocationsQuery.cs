using DirectoryService.Application.Interfaces.CQRS;
using DirectoryService.Contracts.Requests.CommonRequests;
using DirectoryService.Contracts.Responses.LocationResponses;

namespace DirectoryService.Application.CQRS.Queries.Locations.GetLocations;

public sealed record GetLocationsQuery : IQuery<GetLocationsResponse>
{
    public GetLocationsQuery(
        Guid[]? departmentIds,
        ParamsRequest request)
    {
        DepartmentIds = departmentIds;
        Request = request;
    }
    public Guid[]? DepartmentIds { get; init; }
    public ParamsRequest Request { get; init; }
}