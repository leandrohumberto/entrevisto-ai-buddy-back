using Entrevisto.Domain.Entities;

namespace Entrevisto.Application.ViewModels
{
    public class VagaViewModel
    {
        public string? Id { get; set; }
        public required string Titulo { get; set; }
        public required string DescricaoVagaOriginal { get; set; }
        public required string RoteiroPrincipal { get; set; }
        public string? RoteiroTecnico { get; set; }
        public string? RoteiroComportamental { get; set; }
        public string? RoteiroTriagem { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public static VagaViewModel FromVaga(Vaga vaga)
        {
            ArgumentNullException.ThrowIfNull(vaga);

            return new()
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
    }
}
