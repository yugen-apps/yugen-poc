using Poc.Redis.Common.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Poc.Redis.Publisher.Controllers;

[Route("[controller]")]
[ApiController]
public class ApiController : ControllerBase
{
    private readonly IRedisService _redisService;
    private readonly ILogger<ApiController> _logger;

    public ApiController(
        IRedisService redisService,
        ILogger<ApiController> logger)
    {
        _redisService = redisService;
        _logger = logger;
    }

    [HttpGet("status")]
    public async Task<IActionResult> GetStatusAsync()
    {
        return Ok("Poc.Redis.Publisher is up");
    }

    [HttpPost("publish")]
    public async Task<IActionResult> PublishAsync([FromBody] AppMessage forecast)
    {
        await _redisService.PublishAsync(forecast);

        return Ok();
    }

    [HttpPost("produce")]
    public IActionResult Produce([FromBody] AppMessage forecast)
    {
        _redisService.Produce(forecast);

        return Ok();
    }

    [HttpGet("consume")]
    public async Task<IActionResult> ConsumeAsync()
    {
        var result = await _redisService.ConsumeAsync();

        return Ok(result);
    }
}