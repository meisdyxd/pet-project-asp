namespace DirectoryService.Contracts.Respones.CommonResponses;

public record PaginationResponse
{
    public PaginationResponse(
        int page, 
        int pageSize, 
        long totalItems)
    {
        Page = page;
        PageSize = pageSize;
        TotalItems = totalItems;
        TotalPages = (int)Math.Ceiling((double)totalItems / pageSize);
        HasNextPage = Page < TotalPages;
        HasPreviousPage = Page > 1;
    }

    public int Page { get; init; }
    public int PageSize { get; init; }
    public long TotalItems { get; init; }
    public int TotalPages { get; init; }
    public bool HasNextPage { get; init; }
    public bool HasPreviousPage { get; init; }
}