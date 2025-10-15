using Entrevisto.Application.InputModels;
using Entrevisto.Application.ViewModels;

namespace Entrevisto.Application.Services
{
    public interface IOpenAIService
    {
        Task<GenerateScriptViewModel> GenerateScript(GenerateScriptInputModel request);
    }
}
