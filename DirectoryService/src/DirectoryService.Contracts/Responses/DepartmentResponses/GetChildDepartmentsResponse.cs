using DirectoryService.Contracts.Dtos.Departments;
using DirectoryService.Contracts.Responses.CommonResponses;

namespace DirectoryService.Contracts.Responses.DepartmentResponses;

public record GetChildDepartmentsResponse : PaginationResponse
{
    public GetChildDepartmentsResponse(
        IEnumerable<GetChildDepartmentsDto> items,
        int page, 
        int pageSize, 
        long totalItems = 0) : base(page, pageSize, totalItems)
    {
        Items = items;
    }

    public IEnumerable<GetChildDepartmentsDto> Items { get; set; } = [];
}