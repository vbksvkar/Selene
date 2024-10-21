using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Clients.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Models.Configuration;
using Models.Reddit;

namespace Clients.Concretes
{
    public class AuthClient : IAuthClient
    {
        private readonly HttpClient httpClient;
        private readonly SeleneConfig config;
        private readonly ILogger<IAuthClient> logger;

        public AuthClient(HttpClient httpClient, IOptions<SeleneConfig> options, ILogger<IAuthClient> logger)
        {
            this.httpClient = httpClient;
            this.config = options.Value;
            this.logger = logger;
        }
        public async Task<Token> GetTokenAsync()
        {
            var clientId = this.config.ClientId;
            var clientSecret = this.config.ClientSecret;           

            try
            {
                var encodedString = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}"));
                using (var request = new HttpRequestMessage(HttpMethod.Post, this.config.AuthUrl){
                    Headers = { 
                        { "User-Agent", config.UserAgent },
                        { "Authorization", new AuthenticationHeaderValue("Basic", encodedString).ToString() } 
                    },
                    Content = new FormUrlEncodedContent(new[] {
                        new KeyValuePair<string, string>("grant_type", "client_credentials")
                    })
                })
                {
                    var response = await this.httpClient.SendAsync(request);
                    response.EnsureSuccessStatusCode();
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<Token>(content);
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                throw new Exception($"Error while fetching token {httpClient.BaseAddress?.AbsoluteUri}");
            }
        }
    }
}