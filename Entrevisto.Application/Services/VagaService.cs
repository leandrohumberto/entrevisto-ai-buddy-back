using Entrevisto.Application.InputModels;
using Entrevisto.Application.ViewModels;
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

        public async Task<VagaViewModel> CreateVagaAsync(CreateVagaInputModel inputModel, string userId)
        {
            var vaga = inputModel.ToVaga(userId);

            var novaVaga = await _vagaRepository.AddAsync(vaga);

            return VagaViewModel.FromVaga(novaVaga);
        }

        public async Task<bool> DeleteVagaAsync(string id, string userId)
        {
            return await _vagaRepository.DeleteAsync(id, userId);
        }

        public async Task<IEnumerable<VagaViewModel>> GetAllVagasByUserIdAsync(string userId)
        {
            var vagas = await _vagaRepository.GetAllByUserIdAsync(userId);
            
            return vagas.Select(v => VagaViewModel.FromVaga(v));
        }

        public async Task<VagaViewModel> GetVagaByIdAsync(string id, string userId)
        {
            var vaga = await _vagaRepository.GetByIdAsync(id, userId);
            
            if (vaga != null)
            {
                return VagaViewModel.FromVaga(vaga);
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> UpdateVagaAsync(string id, UpdateVagaInputModel inputModel, string userId)
        {
            var vagaExistente = await _vagaRepository.GetByIdAsync(id, userId);
            if (vagaExistente == null) return false;

            vagaExistente.Titulo = inputModel.Titulo;
            vagaExistente.DescricaoVagaOriginal = inputModel.DescricaoVagaOriginal;
            vagaExistente.RoteiroPrincipal = inputModel.RoteiroPrincipal;
            vagaExistente.RoteiroTecnico = inputModel.RoteiroTecnico;
            vagaExistente.RoteiroComportamental = inputModel.RoteiroComportamental;
            vagaExistente.RoteiroTriagem = inputModel.RoteiroTriagem;
            vagaExistente.UpdatedAt = DateTime.UtcNow;

            return await _vagaRepository.UpdateAsync(id, vagaExistente);
        }
    }
}
