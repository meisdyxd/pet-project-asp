using System.Net;
using DirectoryService.Contracts;
using DirectoryService.Contracts.Errors;
using DirectoryService.Domain;
using Microsoft.AspNetCore.Mvc;

namespace DirectoryService.Presentation.Extensions;

public static class ErrorExtensions
{
    public static ActionResult ToResponse(this ErrorList errorList)
    {
        var envelope = Envelope.Failure(errorList.Errors);
        int statusCode = errorList.StatusCode switch
        {
            400 => StatusCodes.Status400BadRequest,
            401 => StatusCodes.Status401Unauthorized,
            403 => StatusCodes.Status403Forbidden,
            404 => StatusCodes.Status404NotFound,
            405 => StatusCodes.Status405MethodNotAllowed,
            406 => StatusCodes.Status406NotAcceptable,
            407 => StatusCodes.Status407ProxyAuthenticationRequired,
            408 => StatusCodes.Status408RequestTimeout,
            409 => StatusCodes.Status409Conflict,
            410 => StatusCodes.Status410Gone,
            _ => StatusCodes.Status500InternalServerError,
        };
        
        return new ObjectResult(envelope)
        {
            StatusCode = statusCode
        };
    }
}