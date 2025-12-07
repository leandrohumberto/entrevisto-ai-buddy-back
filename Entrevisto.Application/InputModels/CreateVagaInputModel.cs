using Entrevisto.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Entrevisto.Application.InputModels
{
    public class CreateVagaInputModel
    {
        [Required]
        public required string Titulo { get; set; }

        [Required]
        public required string DescricaoVagaOriginal { get; set; }

        [Required]
        public required string RoteiroPrincipal { get; set; }

        public string? RoteiroTecnico { get; set; }
        public string? RoteiroComportamental { get; set; }
        public string? RoteiroTriagem { get; set; }

        public Vaga ToVaga(string userId) => new()
        {
            UserId = userId,
            Titulo = Titulo,
            DescricaoVagaOriginal = DescricaoVagaOriginal,
            RoteiroPrincipal = RoteiroPrincipal,
            RoteiroTecnico = RoteiroTecnico,
            RoteiroComportamental = RoteiroComportamental,
            RoteiroTriagem = RoteiroTriagem,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }
}
