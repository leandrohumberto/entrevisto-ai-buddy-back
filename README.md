# Entrevisto AI Buddy - Backend

Este é o backend do projeto Entrevisto AI Buddy, uma API .NET construída para servir a aplicação frontend, lidando com a lógica de negócios, persistência de dados e integração com serviços de IA.

## ✨ Visão Geral

A API é responsável por:
- Gerenciar vagas (CRUD).
- Gerar roteiros de entrevista técnica e comportamental usando o modelo da OpenAI.
- Validar a autenticação de usuários através de tokens JWT emitidos pelo Supabase.

## 🏗️ Arquitetura

O projeto segue os princípios da Clean Architecture, dividido nas seguintes camadas:
- **`Entrevisto.API`**: Camada de apresentação (entrypoint), responsável por expor os endpoints da API.
- **`Entrevisto.Application`**: Contém a lógica de aplicação, serviços e casos de uso.
- **`Entrevisto.Domain`**: Contém as entidades de negócio e as interfaces dos repositórios.
- **`Entrevisto.Infrastructure`**: Implementa os repositórios e a comunicação com serviços externos (banco de dados).

## 🚀 Tecnologias Utilizadas

- **[.NET 8](https://dotnet.microsoft.com/download/dotnet/8.0)** - Framework principal para a construção da API.
- **[ASP.NET Core](https://docs.microsoft.com/aspnet/core)** - Para a criação de endpoints RESTful.
- **[Supabase](https://supabase.io)** - Utilizado para autenticação (validação de JWT).
- **[OpenAI API](https://platform.openai.com)** - Para a geração de conteúdo de IA.

## ⚙️ Configuração e Execução

### Pré-requisitos
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Um editor de código de sua preferência (Visual Studio, VS Code, Rider).

### 1. Configuração do Ambiente
Para rodar o projeto, você precisa configurar as chaves de API e os segredos no arquivo `appsettings.Development.json`.

1.  Navegue até `entrevisto-ai-buddy-back/Entrevisto/Entrevisto.API/`.
2.  Renomeie ou crie uma cópia de `appsettings.json` para `appsettings.Development.json`.
3.  Adicione as seguintes configurações ao arquivo:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "MongoDbSettings": {
    "ConnectionString": "mongodb://localhost:27017",
    "DatabaseName": "EntrevistoDb",
    "CollectionName": "Vagas"
  },
  "OpenAI": {
    "ApiKey": "SUA_API_KEY_DA_OPENAI",
    "Model": "gpt-4o-mini"
  },
  "Supabase": {
    "Authority": "URL_DA_SUA_AUTH_API_SUPABASE",
    "ApiKey": "SUA_SERVICE_ROLE_KEY_SUPABASE"
  }
}
```

**Importante:** A `ApiKey` do Supabase deve ser a sua **`service_role` key**. Ela é necessária para que o backend possa validar os tokens de usuário diretamente com a API do Supabase. Você pode encontrá-la nas configurações de API do seu projeto Supabase.

### 2. Executando a Aplicação
Abra um terminal na pasta raiz do projeto da API (`entrevisto-ai-buddy-back/Entrevisto/Entrevisto.API/`) e execute o seguinte comando:

```bash
dotnet run
```

Por padrão, a API será executada em `http://localhost:5186`.

## Endpoints da API

Todos os endpoints requerem um token de autenticação JWT (Bearer Token) no header da requisição.

- **`POST /api/scripts/generate`**: Gera um roteiro de entrevista.
- **`GET /api/vagas`**: Lista todas as vagas do usuário autenticado.
- **`GET /api/vagas/{id}`**: Obtém os detalhes de uma vaga específica.
- **`POST /api/vagas`**: Cria uma nova vaga.
- **`PUT /api/vagas/{id}`**: Atualiza uma vaga existente.
- **`DELETE /api/vagas/{id}`**: Deleta uma vaga.

