using Entrevisto.Application.DTOs;
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
        public async Task<IActionResult> GetVagas()
        {
            var userId = GetUserId();
            var vagas = await _vagaService.GetAllVagasByUserIdAsync(userId);
            return Ok(vagas);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetVaga(string id)
        {
            var userId = GetUserId();
            var vaga = await _vagaService.GetVagaByIdAsync(id, userId);
            if (vaga == null) return NotFound();
            return Ok(vaga);
        }

        [HttpPost]
        public async Task<IActionResult> CreateVaga([FromBody] CreateVagaDto vagaDto)
        {
            var userId = GetUserId();
            var novaVaga = await _vagaService.CreateVagaAsync(vagaDto, userId);
            return CreatedAtAction(nameof(GetVaga), new { id = novaVaga.Id }, novaVaga);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVaga(string id, [FromBody] UpdateVagaDto vagaDto)
        {
            var userId = GetUserId();
            var success = await _vagaService.UpdateVagaAsync(id, vagaDto, userId);
            if (!success) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVaga(string id)
        {
            var userId = GetUserId();
            var success = await _vagaService.DeleteVagaAsync(id, userId);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}
