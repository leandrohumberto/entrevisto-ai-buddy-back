
using Entrevisto.Application.DTOs;
using Entrevisto.Domain.Entities;
using Entrevisto.Domain.Interfaces;

namespace Entrevisto.Application.Services
{
    public class VagaService : IVagaService
    {
        private readonly IVagaRepository _vagaRepository;

        public VagaService(IVagaRepository vagaRepository)
        {
            _vagaRepository = vagaRepository;
        }

        public async Task<VagaDto> CreateVagaAsync(CreateVagaDto vagaDto, string userId)
        {
            // Mapeamento manual (substituir por AutoMapper depois)
            var vaga = new Vaga
            {
                UserId = userId,
                Titulo = vagaDto.Titulo,
                DescricaoVagaOriginal = vagaDto.DescricaoVagaOriginal,
                RoteiroPrincipal = vagaDto.RoteiroPrincipal,
                RoteiroTecnico = vagaDto.RoteiroTecnico,
                RoteiroComportamental = vagaDto.RoteiroComportamental,
                RoteiroTriagem = vagaDto.RoteiroTriagem,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var novaVaga = await _vagaRepository.AddAsync(vaga);

            // Mapeamento manual de volta para DTO
            return new VagaDto { /* ... preencher propriedades ... */ };
        }

        public async Task<bool> DeleteVagaAsync(string id, string userId)
        {
            return await _vagaRepository.DeleteAsync(id, userId);
        }

        public async Task<IEnumerable<VagaDto>> GetAllVagasByUserIdAsync(string userId)
        {
            var vagas = await _vagaRepository.GetAllByUserIdAsync(userId);
            // Mapeamento manual (substituir por AutoMapper depois)
            return vagas.Select(v => new VagaDto { /* ... preencher ... */ });
        }

        public async Task<VagaDto> GetVagaByIdAsync(string id, string userId)
        {
            var vaga = await _vagaRepository.GetByIdAsync(id, userId);
            // Mapeamento manual (substituir por AutoMapper depois)
            return new VagaDto { /* ... preencher ... */ };
        }

        public async Task<bool> UpdateVagaAsync(string id, UpdateVagaDto vagaDto, string userId)
        {
            var vagaExistente = await _vagaRepository.GetByIdAsync(id, userId);
            if (vagaExistente == null) return false;

            // Mapeamento manual (substituir por AutoMapper depois)
            vagaExistente.Titulo = vagaDto.Titulo;
            // ... outras propriedades
            vagaExistente.UpdatedAt = DateTime.UtcNow;

            return await _vagaRepository.UpdateAsync(id, vagaExistente);
        }
    }
}
