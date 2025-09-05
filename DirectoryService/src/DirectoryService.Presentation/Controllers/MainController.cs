using DirectoryService.Domain;
using Microsoft.AspNetCore.Mvc;

namespace DirectoryService.Presentation.Controllers;

[ApiController]
public class MainController : ControllerBase
{
    public override CreatedResult Created() 
        => Created(string.Empty, Envelope.Success(true));
}