using System.Net;
using BusinessLogic.Interfaces;
using DataStore.Provider;
using Microsoft.Extensions.Logging;
using Models.Response;

namespace BusinessLogic.Concretes
{
    public class RedditService : IRedditService
    {
        private readonly ILogger<IRedditService> logger;
        private readonly IPostsStoreProvider postsStoreProvider;

        public RedditService(
            ILogger<IRedditService> logger,
            IPostsStoreProvider postsStoreProvider)
        {
            this.postsStoreProvider = postsStoreProvider;
            this.logger = logger;
        }

        public async Task<StatsResponse> GetStats(string subRedditName)
        {
            var results = await Task.Run(() => 
            {
                var posts = this.postsStoreProvider.GetStats(subRedditName);
                if (posts == null || posts.Count == 0)
                {
                    return null;
                }

                StatsResponse statsResponse = new StatsResponse()
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    SubredditName = posts.First().SubRedditName,
                    StatsDetails = posts.Select(x => new StatsDetail
                    {
                        StatsText = x.StatsText,
                        StatsItems = x.PostsStats.Select(y => new StatsItem
                        {
                            StatValue = y.Item1,
                            Count = y.Item2
                        }).ToList()
                    }).ToList()
                };
                
                return statsResponse;
            });

            if (results == null)
            {
                this.logger.LogInformation($"No stats found for subreddit {subRedditName}");
                return new StatsResponse
                {
                    StatusCode = (int)HttpStatusCode.NotFound,
                    SubredditName = subRedditName,                    
                    StatsDetails = new List<StatsDetail>()
                    {
                        new StatsDetail { StatsText = "No stats found", StatsItems = new List<StatsItem>() }
                    }
                };
            }

            return results;
        }
    }
}