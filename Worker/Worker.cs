using Clients.Interfaces;
using DataStore.Provider;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Models.Configuration;

namespace Worker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> logger;
        private readonly IRedditClient redditClient;
        private readonly IPostsStoreProvider postsStoreProvider;
        private readonly SeleneConfig config;

        public Worker(
            ILogger<Worker> logger, 
            IRedditClient redditClient,
            IPostsStoreProvider postsStoreProvider,
            IOptions<SeleneConfig> options)
        {
            this.logger = logger;
            this.redditClient = redditClient;
            this.postsStoreProvider = postsStoreProvider;
            this.config = options.Value;
        }
        
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try 
                {
                    var posts = await redditClient.GetPostsAsync();
                    this.postsStoreProvider.AddStats(posts);
                }
                catch (Exception ex)
                {
                    this.logger.LogError(ex, "Error getting posts");
                    while (ex.InnerException != null)
                    {
                        this.logger.LogError(ex.InnerException, "Inner exception");
                        ex = ex.InnerException;
                    }
                }
                await Task.Delay(TimeSpan.FromSeconds(this.config.WorkerInterval), stoppingToken);
            }
        }
    }
}