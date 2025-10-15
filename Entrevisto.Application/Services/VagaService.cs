using Entrevisto.Application.InputModels;
using Entrevisto.Application.ViewModels;
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

        public async Task<VagaViewModel> CreateVagaAsync(CreateVagaInputModel inputModel, string userId)
        {
            var vaga = new Vaga
            {
                UserId = userId,
                Titulo = inputModel.Titulo,
                DescricaoVagaOriginal = inputModel.DescricaoVagaOriginal,
                RoteiroPrincipal = inputModel.RoteiroPrincipal,
                RoteiroTecnico = inputModel.RoteiroTecnico,
                RoteiroComportamental = inputModel.RoteiroComportamental,
                RoteiroTriagem = inputModel.RoteiroTriagem,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var novaVaga = await _vagaRepository.AddAsync(vaga);

            return new VagaViewModel
            {
                Id = novaVaga.Id,
                Titulo = novaVaga.Titulo,
                DescricaoVagaOriginal = novaVaga.DescricaoVagaOriginal,
                RoteiroPrincipal = novaVaga.RoteiroPrincipal,
                RoteiroTecnico = novaVaga.RoteiroTecnico,
                RoteiroComportamental = novaVaga.RoteiroComportamental,
                RoteiroTriagem = novaVaga.RoteiroTriagem,
                CreatedAt = novaVaga.CreatedAt,
                UpdatedAt = novaVaga.UpdatedAt
            };
        }

        public async Task<bool> DeleteVagaAsync(string id, string userId)
        {
            return await _vagaRepository.DeleteAsync(id, userId);
        }

        public async Task<IEnumerable<VagaViewModel>> GetAllVagasByUserIdAsync(string userId)
        {
            var vagas = await _vagaRepository.GetAllByUserIdAsync(userId);
            
            return vagas.Select(v => new VagaViewModel
            {
                Id = v.Id,
                Titulo = v.Titulo,
                DescricaoVagaOriginal = v.DescricaoVagaOriginal,
                RoteiroPrincipal = v.RoteiroPrincipal,
                RoteiroTecnico = v.RoteiroTecnico,
                RoteiroComportamental = v.RoteiroComportamental,
                RoteiroTriagem = v.RoteiroTriagem,
                CreatedAt = v.CreatedAt,
                UpdatedAt = v.UpdatedAt
            });
        }

        public async Task<VagaViewModel> GetVagaByIdAsync(string id, string userId)
        {
            var vaga = await _vagaRepository.GetByIdAsync(id, userId);
            
            if (vaga != null)
            {
                return new VagaViewModel
                {
                    Id = vaga.Id,
                    Titulo = vaga.Titulo,
                    DescricaoVagaOriginal = vaga.DescricaoVagaOriginal,
                    RoteiroPrincipal = vaga.RoteiroPrincipal,
                    RoteiroTecnico = vaga.RoteiroTecnico,
                    RoteiroComportamental = vaga.RoteiroComportamental,
                    RoteiroTriagem = vaga.RoteiroTriagem,
                    CreatedAt = vaga.CreatedAt,
                    UpdatedAt = vaga.UpdatedAt
                };
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
