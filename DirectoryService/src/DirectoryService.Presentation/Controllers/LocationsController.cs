using DirectoryService.Application.CQRS.Commands.AddLocation;
using DirectoryService.Contracts;
using DirectoryService.Presentation.Requests;
using Microsoft.AspNetCore.Mvc;

namespace DirectoryService.Presentation.Controllers;

[ApiController]
[Route("api/locations")]
public class LocationsController : ControllerBase
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
            return BadRequest(result.Error);
        
        return Created();
    }
}