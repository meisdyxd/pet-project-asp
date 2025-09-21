using DirectoryService.Application.CQRS.Commands.Departments.AddDepartment;
using DirectoryService.Application.CQRS.Commands.Departments.TransferDepartment;
using DirectoryService.Application.CQRS.Commands.Departments.UpdateLocations;
using DirectoryService.Contracts.Requests.DepartmentRequests;
using DirectoryService.Presentation.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace DirectoryService.Presentation.Controllers;

[Route("api/departments")]
public class DepartmentsController : MainController
{
    [HttpPost]
    public async Task<IActionResult> Add(
        [FromServices] AddDepartmentCommandHandler handler,
        [FromBody] AddDepartmentRequest request,
        CancellationToken cancellationToken)
    {
        var command = new AddDepartmentCommand(request);
        var result = await handler.Handle(command, cancellationToken);
        if(result.IsFailure)
            return result.Error.ToResponse();
        
        return Created();
    }

    [HttpPut("{departmentId:guid}/locations")]
    public async Task<IActionResult> UpdateLocation(
        [FromRoute] Guid departmentId,
        [FromBody] UpdateLocationsRequest request,
        [FromServices] UpdateLocationsCommandHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new UpdateLocationsCommand(departmentId, request);
        var result = await handler.Handle(command, cancellationToken);
        if(result.IsFailure)
            return result.Error.ToResponse();

        return Ok();
    }

    [HttpPut("{departmentId:guid}/parent")]
    public async Task<IActionResult> ChangeParent(
        [FromRoute] Guid departmentId,
        [FromBody] TransferDepartmentRequest request,
        [FromServices] TransferDepartmentCommandHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new TransferDepartmentCommand(departmentId, request);
        var result = await handler.Handle(command, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok();
    }
}