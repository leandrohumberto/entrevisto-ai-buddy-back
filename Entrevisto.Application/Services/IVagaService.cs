
using Entrevisto.Application.DTOs;

namespace Entrevisto.Application.Services
{
    public interface IVagaService
    {
        Task<VagaDto> GetVagaByIdAsync(string id, string userId);
        Task<IEnumerable<VagaDto>> GetAllVagasByUserIdAsync(string userId);
        Task<VagaDto> CreateVagaAsync(CreateVagaDto vagaDto, string userId);
        Task<bool> UpdateVagaAsync(string id, UpdateVagaDto vagaDto, string userId);
        Task<bool> DeleteVagaAsync(string id, string userId);
    }
}
