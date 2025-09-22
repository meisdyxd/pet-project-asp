namespace DirectoryService.Contracts.Requests.CommonRequests;

public sealed record ParamsRequest
{
    public string[]? SortBy { get; init; }
    public string? SortDirection { get; init; }
    public string? Search { get; init; }
    public bool? IsActive { get; init; }
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}