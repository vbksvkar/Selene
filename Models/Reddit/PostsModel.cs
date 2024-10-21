namespace Models.Reddit
{
    public class PostsModel
    {
        public string SubRedditName { get; set; }
        public string QueryInterval { get; set; }
        public List<RedditPostChild> RedditPostChildren { get; set; } = new List<RedditPostChild>();
    }
}