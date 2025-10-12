using Entrevisto.API.Models;
using Entrevisto.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Entrevisto.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ScriptsController : ControllerBase
    {
        private readonly IOpenAIService _openAIService;

        public ScriptsController(IOpenAIService openAIService)
        {
            _openAIService = openAIService;
        }

        [HttpPost("generate")]
        public async Task<IActionResult> GenerateScript([FromBody] GenerateScriptRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var response = await _openAIService.GenerateScript(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
