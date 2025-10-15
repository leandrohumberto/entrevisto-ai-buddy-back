using Entrevisto.Application.Services;
using Entrevisto.Domain.Interfaces;
using Entrevisto.Infrastructure.Repositories;
using Entrevisto.API.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Configuração de Autenticação JWT com manipulador personalizado
builder.Services.AddHttpClient();
builder.Services.AddAuthentication("Supabase")
    .AddScheme<AuthenticationSchemeOptions, SupabaseAuthenticationHandler>("Supabase", null);

// Injeção de Dependência para Serviços e Repositórios
builder.Services.AddHttpClient<IOpenAIService, OpenAIService>();
builder.Services.AddScoped<IVagaService, VagaService>();
builder.Services.AddSingleton<IVagaRepository, VagaRepository>(); // Singleton para o repositório Mongo

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Entrevisto.API", Version = "v1" });

    // Add JWT security definition
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header usando o schema Bearer."
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                  Type = ReferenceType.SecurityScheme,
                  Id = "Bearer",
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Adiciona os middlewares de autenticação e autorização
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

