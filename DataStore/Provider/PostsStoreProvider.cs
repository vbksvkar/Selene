using Microsoft.Extensions.Logging;
using Models.Extensions;
using Models.Reddit;

namespace DataStore.Provider
{
    public class PostsStoreProvider: IPostsStoreProvider
    {
        private readonly ILogger<PostsStoreProvider> logger;
        private List<PostsModel> posts;

        public PostsStoreProvider(ILogger<PostsStoreProvider> logger)
        {
            this.logger = logger;
        }

        public void AddStats(List<PostsModel> posts)
        {
            this.posts = posts;
        }

        public List<PostsStatsModel> GetStats(string subredditName)
        {
            List<PostsStatsModel> results = new List<PostsStatsModel>();
            if (posts == null || !posts.Any())
            {
                return results;
            }

            var postsForSubreddit = posts.FirstOrDefault(x => x.SubRedditName.Equals(subredditName, StringComparison.OrdinalIgnoreCase));
            if (postsForSubreddit == null)
            {
                return results;
            }

            // Top Posts
            PostsStatsModel topPostsModel = new PostsStatsModel()
            {
                StatsText = $"Posts with most up-votes this {postsForSubreddit.QueryInterval}",
                SubRedditName = postsForSubreddit.SubRedditName,
                PostsStats = postsForSubreddit.RedditPostChildren
                    .OrderByDescending(x => x.RedditPost.UpVotes)
                    .Select(x => Tuple.Create(x.RedditPost.Title, x.RedditPost.UpVotes))
                    .ToList()
            };
            results.Add(topPostsModel);

            PostsStatsModel topUserPostsModel = new PostsStatsModel()
            {
                StatsText = $"Users with most posts this {postsForSubreddit.QueryInterval}",
                SubRedditName = postsForSubreddit.SubRedditName,
                PostsStats = postsForSubreddit.RedditPostChildren
                    .GroupBy(x => x.RedditPost.Author)
                    .Where(x => x.Count() > 1)
                    .OrderByDescending(x => x.Count())
                    .Select(x => Tuple.Create(x.Key, x.Count()))
                    .ToList()
            };
            results.Add(topUserPostsModel);

            PostsStatsModel crossPostsModel = new PostsStatsModel
            {
                StatsText = $"Posts with most cross postings this {postsForSubreddit.QueryInterval}",
                SubRedditName = postsForSubreddit.SubRedditName,
                PostsStats = postsForSubreddit.RedditPostChildren
                    .Where(x => x.RedditPost.CrossPosted > 10)
                    .OrderByDescending(x => x.RedditPost.CrossPosted)
                    .Select(x => Tuple.Create(x.RedditPost.Title, x.RedditPost.CrossPosted))
                    .ToList()
            };            
            results.Add(crossPostsModel);

            return results;
        }        
    }
}