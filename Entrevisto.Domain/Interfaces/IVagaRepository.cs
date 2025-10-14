
using Entrevisto.Domain.Entities;

namespace Entrevisto.Domain.Interfaces
{
    public interface IVagaRepository
    {
        Task<Vaga> GetByIdAsync(string id, string userId);
        Task<IEnumerable<Vaga>> GetAllByUserIdAsync(string userId);
        Task<Vaga> AddAsync(Vaga vaga);
        Task<bool> UpdateAsync(string id, Vaga vaga);
        Task<bool> DeleteAsync(string id, string userId);
    }
}
