using System.ComponentModel.DataAnnotations;

namespace Entrevisto.Application.InputModels
{
    public class UpdateVagaInputModel
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
    }
}
