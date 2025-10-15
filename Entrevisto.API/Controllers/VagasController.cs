using Entrevisto.Application.InputModels;
using Entrevisto.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Entrevisto.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class VagasController : ControllerBase
    {
        private readonly IVagaService _vagaService;

        public VagasController(IVagaService vagaService)
        {
            _vagaService = vagaService;
        }

        private string GetUserId() => User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var userId = GetUserId();
            var vagas = await _vagaService.GetAllVagasByUserIdAsync(userId);
            return Ok(vagas);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var userId = GetUserId();
            var vaga = await _vagaService.GetVagaByIdAsync(id, userId);
            if (vaga == null) return NotFound();
            return Ok(vaga);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateVagaInputModel inputModel)
        {
            var userId = GetUserId();
            var novaVaga = await _vagaService.CreateVagaAsync(inputModel, userId);
            return CreatedAtAction(nameof(GetById), new { id = novaVaga.Id }, novaVaga);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] UpdateVagaInputModel inputModel)
        {
            var userId = GetUserId();
            var success = await _vagaService.UpdateVagaAsync(id, inputModel, userId);
            if (!success) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var userId = GetUserId();
            var success = await _vagaService.DeleteVagaAsync(id, userId);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}
