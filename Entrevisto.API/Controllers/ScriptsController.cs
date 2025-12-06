using Entrevisto.Application.InputModels;
using Entrevisto.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Entrevisto.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ScriptsController : ControllerBase
    {
        private readonly IAiService _aiService;

        public ScriptsController(IAiService aiService)
        {
            _aiService = aiService;
        }

        [HttpPost("generate")]
        public async Task<IActionResult> GenerateScript([FromBody] GenerateScriptInputModel request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var response = await _aiService.GenerateScript(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
