using Entrevisto.Application.InputModels;
using Entrevisto.Application.ViewModels;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Entrevisto.Application.Services
{
    public class OpenAIService : IOpenAIService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public OpenAIService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<GenerateScriptViewModel> GenerateScript(GenerateScriptInputModel request)
        {
            var apiKey = _configuration["OpenAI:ApiKey"];
            if (string.IsNullOrEmpty(apiKey))
            {
                throw new Exception("Key not found in configuration.");
            }

            var openAiModel = _configuration["OpenAI:Model"];
            if (string.IsNullOrEmpty(openAiModel))
            {
                throw new Exception("Model not found in configuration.");
            }

            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiKey);

            var (systemPrompt, userPrompt) = GetPrompts(request);

            var openAIRequest = new
            {
                model = openAiModel,
                messages = new[]
                {
                    new { role = "system", content = systemPrompt },
                    new { role = "user", content = userPrompt }
                },
                temperature = 0.7,
            };

            var response = await _httpClient.PostAsJsonAsync("https://api.openai.com/v1/chat/completions", openAIRequest);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error calling API: {error}");
            }

            var openAIResponse = await response.Content.ReadFromJsonAsync<OpenAIResponse>();

            return new GenerateScriptViewModel
            {
                Script = openAIResponse.choices[0].message.content
            };
        }

        private static (string, string) GetPrompts(GenerateScriptInputModel request)
        {
            string systemPrompt;
            string userPrompt;

            if (request.RegenerationType == RegenerationType.MainScript)
            {
                systemPrompt = "Você é um especialista em Recrutamento e Seleção com mais de 15 anos de experiência, especializado em criar roteiros de entrevista eficazes.\nSua tarefa é analisar a descrição de vaga fornecida pelo usuário e gerar um roteiro de entrevista estruturado e profissional.\n\n**Sua Missão:**\n\n1. **Analise o Texto:** Identifique as principais hard skills (tecnologias, ferramentas), soft skills (competências comportamentais), o nível de senioridade e a área de atuação da vaga.\n2. **Crie um Roteiro Estruturado:** Organize o roteiro nas seguintes seções:\n   * Abertura e Quebra-Gelo\n   * Validação Técnica (Hard Skills)\n   * Entrevista Comportamental (Soft Skills)\n   * Alinhamento Cultural e Motivacional\n3. **Gere Perguntas Inteligentes:**\n   * Para **Hard Skills**, crie perguntas práticas.\n   * Para **Soft Skills**, use o modelo **STAR (Situação, Tarefa, Ação, Resultado)**.\n   * As perguntas de **Abertura** devem ser acolhedoras.\n   * As de **Alinhamento** devem explorar motivações e fit cultural.\n4. **Adicione Explicações:** Para **cada pergunta**, adicione uma linha \"**Objetivo da Pergunta:**\" explicando o que está sendo avaliado.\n5. **Finalize o Roteiro:** Com uma seção de **Encerramento**, lembrando o entrevistador de abrir espaço para perguntas do candidato.\n\n**Formato da Saída:**\nUse **Markdown** com títulos claros e seções bem organizadas.\nNão inclua comentários extras, apenas o roteiro final.";
                userPrompt = $"**Descrição da Vaga:**\n\n{request.JobDescription}";
            }
            else if (request.RegenerationType == RegenerationType.Technical)
            {
                systemPrompt = "# INSTRUÇÃO DE REGENERAÇÃO: APROFUNDAR ANÁLISE TÉCNICA\n\nVocê é um especialista em Recrutamento e Seleção e JÁ gerou um roteiro de entrevista inicial. O usuário agora solicitou uma versão com uma análise técnica **MUITO MAIS PROFUNDA**, adequada para avaliar um candidato de nível Pleno a Sênior.\n\n**Sua Nova Missão:**\n\n1. **Foco Total na Técnica:** Sua principal tarefa é **REESCREVER COMPLETAMENTE** a seção \"Validação Técnica (Hard Skills)\" do roteiro. Mantenha as outras seções (Abertura, Comportamental, etc.) exatamente como estavam ou resuma-as drasticamente para dar espaço à parte técnica.\n2. **Mude a Natureza das Perguntas:**\n   * Transforme perguntas de \"o que é X?\" ou \"você já usou Y?\" em perguntas de \"por que\" e \"quando\".\n   * Crie **cenários hipotéticos ou pequenos cases** que forcem o candidato a desenhar uma solução.\n   * Adicione perguntas que explorem **escalabilidade, performance, segurança e boas práticas**.\n   * Formule questões que peçam ao candidato para **comparar ferramentas ou arquiteturas**.\n3. **Mantenha o Objetivo em Mente:** Avaliar a **profundidade técnica, raciocínio crítico e capacidade de argumentação**.\n4. **Preserve o Formato:** Mantenha o \"Objetivo da Pergunta\" para cada nova pergunta.";
                userPrompt = $"**Descrição da Vaga:**\n\n{request.JobDescription}\n\n**Roteiro Gerado Anteriormente:**\n\n{request.PreviousScript}";
            }
            else if (request.RegenerationType == RegenerationType.Behavioral)
            {
                systemPrompt = "# INSTRUÇÃO DE REGENERAÇÃO: FOCAR EM COMPORTAMENTAL\n\nVocê é um especialista em Recrutamento e Seleção e JÁ gerou um roteiro de entrevista inicial. O usuário agora solicitou uma versão com **FOCO EXPANDIDO** na entrevista comportamental, ideal para avaliar soft skills, fit cultural e potencial de liderança.\n\n**Sua Nova Missão:**\n\n1. **Priorize o Comportamento:** **EXPANDA SIGNIFICATIVAMENTE** a seção \"Entrevista Comportamental (Soft Skills)\" adicionando de 5 a 7 novas perguntas aprofundadas.\n2. **Condense a Técnica:** Reduza \"Validação Técnica (Hard Skills)\" a apenas 2 ou 3 perguntas de alto nível.\n3. **Aumente a Variedade das Competências:**\n   * Explore competências como **resiliência**, **gestão de tempo**, **proatividade**, **aprendizado contínuo**, **feedback** e, se aplicável, **liderança**.\n4. **Mantenha o Objetivo em Mente:** Avaliar **como** o candidato age em situações reais, seu estilo de comunicação e colaboração.\n5. **Preserve o Formato:** Inclua o \"Objetivo da Pergunta\" em cada item.";
                userPrompt = $"**Descrição da Vaga:**\n\n{request.JobDescription}\n\n**Roteiro Gerado Anteriormente:**\n\n{request.PreviousScript}";
            }
            else if (request.RegenerationType == RegenerationType.Screening)
            {
                systemPrompt = "# INSTRUÇÃO DE REGENERAÇÃO: VERSÃO PARA TRIAGEM (SCREENING)\n\nVocê é um especialista em Recrutamento e Seleção e JÁ gerou um roteiro de entrevista inicial. O usuário agora solicitou uma versão **CONCISA e OTIMIZADA** para uma **entrevista de triagem (screening)** com duração máxima de 15 a 20 minutos.\n\n**Sua Nova Missão:**\n\n1. **Seja Breve e Direto:** **REESTRUTURE E CONDENSE RADICALMENTE** o roteiro, eliminando aprofundamentos.\n2. **Crie Novas Seções de Triagem:**\n   * **1. Breve Apresentação e Alinhamento** — 1 pergunta sobre experiência e motivação.\n   * **2. Validação de Requisitos Chave** — 2–3 perguntas rápidas sobre competências principais.\n   * **3. Alinhamento de Carreira e Motivação** — 1–2 perguntas abertas.\n   * **4. Questões Logísticas (Checklist)** — placeholders para salário, disponibilidade, modelo de trabalho e dúvidas do candidato.\n3. **Mantenha o Objetivo em Mente:** Focar em **qualificação rápida** e **fit inicial**.\n4. **Simplifique o Formato:** Checklist ou roteiro enxuto em Markdown.";
                userPrompt = $"**Descrição da Vaga:**\n\n{request.JobDescription}\n\n**Roteiro Gerado Anteriormente:**\n\n{request.PreviousScript}";
            }
            else
            {
                throw new Exception($"Regeneration type no defined");
            }
            
            return (systemPrompt, userPrompt);
        }

        private class OpenAIResponse
        {
            public Choice[] choices { get; set; }
        }

        private class Choice
        {
            public Message message { get; set; }
        }

        private class Message
        {
            public string content { get; set; }
        }
    }
}
