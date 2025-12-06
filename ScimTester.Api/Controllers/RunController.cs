using Microsoft.AspNetCore.Mvc;
using ScimTester.Core;
using ScimTester.Core.Models;

namespace ScimTester.Api.Controllers;
[ApiController]
[Route("api/run")]
public class RunController : ControllerBase
{
    private readonly IScimRunner _runner;
    public RunController(IScimRunner runner) => _runner = runner;

    [HttpPost("run")]
    public async Task<IActionResult> Run([FromBody] RunRequest request)
    {
        try
        {
            if (string.IsNullOrEmpty(request.BaseUrl))
                return BadRequest("BaseUrl is required");
                
            var report = await _runner.RunSuiteAsync(request);
            return Ok(report);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message, stackTrace = ex.StackTrace });
        }
    }

    [HttpGet("{runId}")]
    public IActionResult Get(string runId)
    {
        // MVP: no persistence; would return 404
        return NotFound(new { message = "Run persistence not implemented in MVP scaffold." });
    }
}
