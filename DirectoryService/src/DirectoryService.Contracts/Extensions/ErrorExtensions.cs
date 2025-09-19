using DirectoryService.Contracts.Errors;

namespace DirectoryService.Contracts.Extensions;

public static class ErrorExtensions
{
    public static ErrorList ToErrorList(this Error error, int statusCode = 400)
    {
        return new ErrorList([error], statusCode);
    }
}