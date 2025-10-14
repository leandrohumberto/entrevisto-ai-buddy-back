
using System.ComponentModel.DataAnnotations;

namespace Entrevisto.Application.DTOs
{
    public class UpdateVagaDto
    {
        [Required]
        public string Titulo { get; set; }

        [Required]
        public string DescricaoVagaOriginal { get; set; }

        [Required]
        public string RoteiroPrincipal { get; set; }

        public string? RoteiroTecnico { get; set; }
        public string? RoteiroComportamental { get; set; }
        public string? RoteiroTriagem { get; set; }
    }
}
