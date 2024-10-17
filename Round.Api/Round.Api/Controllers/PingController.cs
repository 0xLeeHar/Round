using Microsoft.AspNetCore.Mvc;

namespace Round.Api.Controllers;

[ApiController]
[Route("api/ping")]
public class PingController : ControllerBase
{
    private readonly ILogger<PingController> _logger;

    public PingController(ILogger<PingController> logger)
    {
        _logger = logger;
    }
    
    [HttpGet]
    public string Get()
    {
        _logger.LogTrace("Ping called");
        return "pong";
    }
}