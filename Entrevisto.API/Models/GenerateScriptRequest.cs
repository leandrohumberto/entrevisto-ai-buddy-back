
namespace Entrevisto.API.Models
{
    public class GenerateScriptRequest
    {
        public string JobDescription { get; set; }
        public string? RegenerationType { get; set; }
        public string? PreviousScript { get; set; }
    }
}
