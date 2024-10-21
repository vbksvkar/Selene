using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Clients.Providers
{
    public class RateLimitingProvider : DelegatingHandler
    {
        private const string RateLimitUsedKey = "x-ratelimit-used";
        private const string RateLimitRemainingKey = "x-ratelimit-remaining";
        private const string RateLimitResetKey = "x-ratelimit-reset";

        private readonly ILogger<RateLimitingProvider> logger;
        private int? requestsUsed;
        private double? requestsRemaining;
        private int? requestsReset;
        
        public RateLimitingProvider(ILogger<RateLimitingProvider> logger)
        {
            this.logger = logger;            
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (this.requestsUsed.HasValue && this.requestsRemaining.HasValue && this.requestsRemaining < 1)
            {
                var pauseSeconds = TimeSpan.FromSeconds(this.requestsReset.Value);
                this.logger.LogInformation($"Rate limit reached, pausing for {pauseSeconds}");
                await Task.Delay(pauseSeconds, cancellationToken);
            }

            var response = await base.SendAsync(request, cancellationToken);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                ParseRateLimitHeaders(response.Headers);
            }
            return response;
        }

        private void ParseRateLimitHeaders(HttpResponseHeaders headers)
        {            
            var requestsUsedExists = headers.TryGetValues(RateLimitUsedKey, out var requestUsedValues);
            if (requestsUsedExists)
            {
                requestsUsed = Convert.ToInt16(requestUsedValues.First());
            }

            var requestRemainingExists = headers.TryGetValues(RateLimitRemainingKey, out var requestRemainingValues);
            if (requestRemainingExists)
            {
                requestsRemaining = Convert.ToDouble(requestRemainingValues.First());
            }

            var requestsResetExists = headers.TryGetValues(RateLimitResetKey, out var requestResetValues);
            if (requestsResetExists)
            {
                requestsReset = Convert.ToInt16(requestResetValues.First());
            }
        }


    }
}