using DirectoryService.Contracts.Dtos.Departments;
using DirectoryService.Contracts.Responses.CommonResponses;

namespace DirectoryService.Contracts.Responses.DepartmentResponses;

public sealed record GetRootDepartmentsWithChildsResponse : PaginationResponse
{
    public GetRootDepartmentsWithChildsResponse(
        IEnumerable<GetRootDepartmentsDto> items,
        int page, 
        int pageSize, 
        long totalItems = 0) : base(page, pageSize, totalItems)
    {
        Items = items;
    }
    
    public IEnumerable<GetRootDepartmentsDto> Items { get; init; } = [];
}