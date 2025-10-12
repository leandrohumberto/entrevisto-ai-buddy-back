using Entrevisto.API.Models;

namespace Entrevisto.API.Services
{
    public interface IOpenAIService
    {
        Task<GenerateScriptResponse> GenerateScript(GenerateScriptRequest request);
    }
}
