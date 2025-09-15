using DirectoryService.Application.CQRS.Commands.AddDepartment;
using DirectoryService.Presentation.Extensions;
using DirectoryService.Presentation.Requests;
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
        var command = request.ToCommand();
        var result = await handler.Handle(command, cancellationToken);
        if(result.IsFailure)
            return result.Error.ToResponse();
        
        return Created();
    }
}