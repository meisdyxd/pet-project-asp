using DirectoryService.Application.Interfaces.CQRS;
using DirectoryService.Contracts.Responses.DepartmentResponses;

namespace DirectoryService.Application.CQRS.Queries.Departments.GetRootDepartmentsWithChilds;

public sealed record GetRootDepartmentsWithChildsQuery : IQuery<GetRootDepartmentsWithChildsResponse>
{
    public GetRootDepartmentsWithChildsQuery(
        int? page,
        int? pageSize,
        int? prefetch)
    {
        Page = page ?? 1;
        PageSize = pageSize ?? 20;
        Prefetch = prefetch ?? 3;
    }
    
    public int Page { get; init; }
    public int PageSize { get; init; }
    public int Prefetch { get; init; }
}