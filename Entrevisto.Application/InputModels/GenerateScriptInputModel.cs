namespace Entrevisto.Application.InputModels
{
    public class GenerateScriptInputModel
    {
        public required string JobDescription { get; set; }
        public RegenerationType RegenerationType { get; set; }
        public string? PreviousScript { get; set; }
    }

    public enum RegenerationType
    {
        MainScript = 1,
        Technical = 2,
        Behavioral = 3,
        Screening = 4,
    }
}
