using System.Text.Json.Serialization;

namespace Models.Reddit
{
    public class RedditPostChild
    {
        [JsonPropertyName("data")]
        public RedditPost RedditPost { get; set; }

        [JsonPropertyName("kind")]
        public string PostKind { get; set; }
    }
}