using Entrevisto.Application.InputModels;
using Entrevisto.Application.ViewModels;

namespace Entrevisto.Application.Services
{
    public interface IVagaService
    {
        Task<VagaViewModel> GetVagaByIdAsync(string id, string userId);
        Task<IEnumerable<VagaViewModel>> GetAllVagasByUserIdAsync(string userId);
        Task<VagaViewModel> CreateVagaAsync(CreateVagaInputModel vagaDto, string userId);
        Task<bool> UpdateVagaAsync(string id, UpdateVagaInputModel vagaDto, string userId);
        Task<bool> DeleteVagaAsync(string id, string userId);
    }
}
