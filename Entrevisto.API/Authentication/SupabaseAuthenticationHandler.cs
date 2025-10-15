using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System;

namespace Entrevisto.API.Authentication
{
    public class SupabaseAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public SupabaseAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration)
            : base(options, logger, encoder)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.TryGetValue("Authorization", out Microsoft.Extensions.Primitives.StringValues value))
            {
                return AuthenticateResult.Fail("Authorization header not found.");
            }

            var authHeader = AuthenticationHeaderValue.Parse(value);

            if (!authHeader.Scheme.Equals("bearer", StringComparison.CurrentCultureIgnoreCase))
            {
                return AuthenticateResult.Fail("Invalid authentication scheme.");
            }

            var token = authHeader.Parameter;
            if (string.IsNullOrEmpty(token))
            {
                return AuthenticateResult.Fail("Missing token.");
            }

            var supabaseUrl = _configuration["Supabase:Authority"];
            var supabaseApiKey = _configuration["Supabase:ApiKey"];

            if (string.IsNullOrEmpty(supabaseUrl) || string.IsNullOrEmpty(supabaseApiKey))
            {
                return AuthenticateResult.Fail("Configuration is missing");
            }

            try
            {
                var client = _httpClientFactory.CreateClient();
                var requestMessage = new HttpRequestMessage(HttpMethod.Get, $"{supabaseUrl}/user");
                requestMessage.Headers.Add("apikey", supabaseApiKey);
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await client.SendAsync(requestMessage);

                if (!response.IsSuccessStatusCode)
                {
                    return AuthenticateResult.Fail("Token validation failed.");
                }

                var content = await response.Content.ReadAsStringAsync();
                using var userDoc = JsonDocument.Parse(content);

                var claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, userDoc.RootElement.GetProperty("id").GetString()),
                    new Claim(ClaimTypes.Email, userDoc.RootElement.GetProperty("email").GetString()),
                };

                var identity = new ClaimsIdentity(claims, Scheme.Name);
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, Scheme.Name);

                return AuthenticateResult.Success(ticket);
            }
            catch (Exception ex)
            {
                return AuthenticateResult.Fail($"An exception occurred during token validation: {ex.Message}");
            }
        }
    }
}
