using Entrevisto.Application.InputModels;
using Entrevisto.Application.ViewModels;

namespace Entrevisto.Application.Services
{
    public interface IAiService
    {
        Task<GenerateScriptViewModel> GenerateScript(GenerateScriptInputModel request);
    }
}
