using System.Net;

namespace DirectoryService.Contracts.Errors;

public class ErrorList
{
    public ErrorList(IEnumerable<Error> errors, int statusCode)
    {
        Errors = errors;
        StatusCode = statusCode;
    }
    public ErrorList(IEnumerable<Error> errors, HttpStatusCode statusCode)
    {
        Errors = errors;
        StatusCode = (int)statusCode;
    }
    
    public IEnumerable<Error> Errors { get; set; }
    public int StatusCode { get; set; }
}