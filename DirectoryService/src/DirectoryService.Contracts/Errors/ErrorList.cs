using System.Net;

namespace DirectoryService.Contracts;

public class ErrorList
{
    public ErrorList(IEnumerable<Error> errors, int statusCode)
    {
        Errors = errors;
        StatusCode = statusCode;
    }
    
    public IEnumerable<Error> Errors { get; set; }
    public int StatusCode { get; set; }
}