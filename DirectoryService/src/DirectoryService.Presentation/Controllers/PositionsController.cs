using DirectoryService.Application.CQRS.Commands.AddPosition;
using DirectoryService.Presentation.Extensions;
using DirectoryService.Presentation.Requests;
using Microsoft.AspNetCore.Mvc;

namespace DirectoryService.Presentation.Controllers;

[Route("api/positions")]
public class PositionsController : MainController
{
    [HttpPost]
    public async Task<IActionResult> Add(
        [FromServices] AddPositionCommandHandler handler,
        [FromBody] AddPositionRequest request,
        CancellationToken cancellationToken)
    {
        var command = request.ToCommand();
        var result = await handler.Handle(command, cancellationToken);
        if(result.IsFailure)
            return result.Error.ToResponse();
        
        return Created();
    }
}