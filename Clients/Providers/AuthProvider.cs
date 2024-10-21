using System.Net.Http.Headers;
using Clients.Interfaces;
using Microsoft.Extensions.Logging;
using Models.Reddit;

namespace Clients.Providers
{
    public class AuthProvider : DelegatingHandler
    {
        private readonly ILogger<AuthProvider> logger;
        private readonly IAuthClient authClient;
        private Token redditToken;

        public AuthProvider(ILogger<AuthProvider> logger, IAuthClient authClient)
        {
            this.logger = logger;
            this.authClient = authClient;
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, 
            CancellationToken cancellationToken)
        {
            if (this.redditToken == null)
            {
                this.logger.LogInformation("Fetching token");
                this.redditToken = await this.authClient.GetTokenAsync();
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", this.redditToken.AccessToken);
                return await base.SendAsync(request, cancellationToken);
            }

            var timeDiff = this.redditToken.ExpiryTime.Value - DateTime.UtcNow;
            if (timeDiff.TotalSeconds < 60)
            {
                this.logger.LogInformation("Token expired, fetching new token");
                this.redditToken = await this.authClient.GetTokenAsync();
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", this.redditToken.AccessToken);
                return await base.SendAsync(request, cancellationToken);
            }

            return await base.SendAsync(request, cancellationToken);
        }

    }
}