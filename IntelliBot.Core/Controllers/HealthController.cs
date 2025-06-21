using IntelliBot.Core.Models;
using IntelliBot.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace IntelliBot.Core.Controllers;

[ApiController]
[Route("[controller]")]
public class HealthController : ControllerBase
{
    [HttpGet]
    public IActionResult Get() => Ok("Healthy");
}