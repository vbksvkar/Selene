using System.Text.Json.Serialization;
using Models.Extensions;

namespace Models.Reddit
{
    public class RedditPost
    {
        [JsonPropertyName("subreddit_name_prefixed")]
        public string SubRedditName { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("downs")]
        public int DownVotes { get; set; }

        [JsonPropertyName("ups")]
        public int UpVotes { get; set; }

        [JsonPropertyName("author")]
        public string Author { get; set; }

        [JsonPropertyName("num_comments")]
        public int CommentsCount { get; set; }

        [JsonPropertyName("created_utc")]
        public double CreatedUtcEpoch { get; set; }

        [JsonPropertyName("num_crossposts")]
        public int CrossPosted { get; set; }

        [JsonIgnore]
        public DateTime CreatedUtc
        {
            get
            {
                return this.CreatedUtcEpoch.FromEpochTime();
            }
        }
    }
}