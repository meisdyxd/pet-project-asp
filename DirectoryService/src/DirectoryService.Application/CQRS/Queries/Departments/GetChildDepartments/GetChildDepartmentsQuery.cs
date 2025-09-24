using DirectoryService.Application.Interfaces.CQRS;
using DirectoryService.Contracts.Responses.DepartmentResponses;

namespace DirectoryService.Application.CQRS.Queries.Departments.GetChildDepartments;

public sealed record GetChildDepartmentsQuery : IQuery<GetChildDepartmentsResponse>
{
    public GetChildDepartmentsQuery(
        Guid parentId,
        int? page,
        int? pageSize)
    {
        ParentId = parentId;
        Page = page ?? 1;
        PageSize = pageSize ?? 20;
    }
    
    public Guid ParentId { get; init; }
    public int Page { get; init; }
    public int PageSize { get; init; }
}