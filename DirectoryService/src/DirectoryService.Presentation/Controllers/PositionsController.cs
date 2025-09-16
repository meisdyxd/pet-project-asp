using DirectoryService.Application.CQRS.Commands.Positions.AddPosition;
using DirectoryService.Contracts.Requests;
using DirectoryService.Contracts.Requests.PositionRequests;
using DirectoryService.Presentation.Extensions;
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
        var command = new AddPositionCommand(request);
        var result = await handler.Handle(command, cancellationToken);
        if(result.IsFailure)
            return result.Error.ToResponse();
        
        return Created();
    }
}