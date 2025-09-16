using DirectoryService.Application.CQRS.Commands.Locations.AddLocation;
using DirectoryService.Contracts.Requests;
using DirectoryService.Contracts.Requests.LocationRequests;
using DirectoryService.Presentation.Extensions;
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
        var command = new AddLocationCommand(request);
        var result = await handler.Handle(command, cancellationToken);
        if(result.IsFailure)
            return result.Error.ToResponse();
        
        return Created();
    }
}