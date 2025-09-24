using DirectoryService.Contracts.Dtos.Departments;

namespace DirectoryService.Contracts.Responses.DepartmentResponses;

public sealed record GetWithTopPositionsResponse
{
    public GetWithTopPositionsResponse(IEnumerable<GetWithTopPositionsDto> departments)
    {
        Items = departments;
    }
    
    public IEnumerable<GetWithTopPositionsDto> Items { get; init; } = [];
}