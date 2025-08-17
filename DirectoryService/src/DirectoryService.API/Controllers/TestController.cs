using Microsoft.AspNetCore.Mvc;

namespace DirectoryService.Presentation.Controllers;


[ApiController]
[Route("api/[controller]")]
public class TestController: ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Test()
    {
        return await Task.FromResult(Ok("Hello world!"));
    }
}
