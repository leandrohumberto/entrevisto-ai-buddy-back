namespace Entrevisto.Application.ViewModels
{
    public class VagaViewModel
    {
        public string Id { get; set; }
        public string Titulo { get; set; } 
        public string DescricaoVagaOriginal { get; set; }
        public string RoteiroPrincipal { get; set; }
        public string? RoteiroTecnico { get; set; }
        public string? RoteiroComportamental { get; set; }
        public string? RoteiroTriagem { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
