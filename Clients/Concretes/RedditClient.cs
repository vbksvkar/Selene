using System.Net;
using System.Runtime.CompilerServices;
using System.Text.Json;
using Clients.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Models.Configuration;
using Models.Reddit;

namespace Clients.Concretes
{
    public class RedditClient : IRedditClient
    {
        private readonly HttpClient httpClient;
        private readonly ILogger<IRedditClient> logger;
        private readonly SeleneConfig config;
        private readonly List<string> allowedIntervals = new List<string> { "hour", "day", "week", "month", "year", "all" };

        public RedditClient(HttpClient httpClient, ILogger<IRedditClient> logger, IOptions<SeleneConfig> options)
        {
            this.httpClient = httpClient;
            this.logger = logger;
            this.config = options.Value;
        }
        public async Task<List<PostsModel>> GetPostsAsync()
        {
            this.httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Pallas/1.0");
            List<PostsModel> results = new List<PostsModel>();
            foreach (var item in this.config.SubredditConfig)
            {
                if (!this.allowedIntervals.Contains(item.Interval))
                {
                    this.logger.LogError($"Invalid interval {item.Interval} for {item.Name}, skipping..");
                    continue;
                }

                string url = $"{this.config.RedditUrl}/{item.Name}/top.json?limit={item.PostsCount}&t={item.Interval}";
                try 
                {
                    var response = await this.httpClient.GetAsync(url);
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        var errorContent = await response.Content.ReadAsStringAsync();
                        this.logger.LogError($"Error getting posts from {item.Name}, status code: {response.StatusCode}");                        
                        this.logger.LogError(errorContent);
                        continue;
                    }

                    //An invalid subreddit name returns 200 OK (for e.g. r/werwe)
                    var content = await response.Content.ReadAsStringAsync();
                    RedditResponse redditResponse = JsonSerializer.Deserialize<RedditResponse>(content);
                    if (redditResponse?.RedditData?.Children?.Count > 0)
                    {
                        var subRedditName = redditResponse.RedditData.Children[0].RedditPost.SubRedditName;
                        results.Add(new PostsModel
                        {
                            SubRedditName = subRedditName,
                            QueryInterval = item.Interval,
                            RedditPostChildren = redditResponse.RedditData.Children
                        });
                    }
                }
                catch (Exception ex)
                {
                    this.logger.LogError(ex, $"Error getting posts from {item.Name}");
                    throw new Exception($"Error getting posts from {item.Name}");
                }
            }
            return results;
        }
    }
}