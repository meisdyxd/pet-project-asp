using DirectoryService.Application.CQRS.Commands.Locations.AddLocation;
using DirectoryService.Application.CQRS.Queries.Locations.GetLocations;
using DirectoryService.Contracts.Requests;
using DirectoryService.Contracts.Requests.CommonRequests;
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

    [HttpGet]
    public async Task<IActionResult> Get(
        [FromQuery] Guid[]? departmentId,
        [FromQuery] ParamsRequest paramsRequest,
        [FromServices] GetLocationsQueryHandler handler,
        CancellationToken cancellationToken)
    {
        var query = new GetLocationsQuery(departmentId, paramsRequest);
        var result = await handler.Handle(query, cancellationToken);
        if(result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok(result.Value);
    }
}