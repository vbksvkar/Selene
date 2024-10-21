namespace Models.Configuration
{
    public class SeleneConfig
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string AuthUrl { get; set; }
        public string RedditUrl { get; set; }
        public List<SubredditConfig> SubredditConfig { get; set; }
        public string UserAgent { get; set; }
        public int WorkerInterval { get; set; }
    }
}