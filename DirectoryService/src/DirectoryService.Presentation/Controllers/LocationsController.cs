using DirectoryService.Application.CQRS.Commands.AddLocation;
using DirectoryService.Presentation.Extensions;
using DirectoryService.Presentation.Requests;
using Microsoft.AspNetCore.Mvc;

namespace DirectoryService.Presentation.Controllers;

[Route("api/locations")]
public class LocationsController : MainController
{
    [HttpPost]
    public async Task<IActionResult> Add(
        [FromBody] AddLocationRequest request,
        [FromServices] AddLocationCommandHandler handler,
        CancellationToken cancellationToken)
    {
        var command = request.ToCommand();
        var result = await handler.Handle(command, cancellationToken);
        if(result.IsFailure)
            return result.Error.ToResponse();
        
        return Created();
    }
}