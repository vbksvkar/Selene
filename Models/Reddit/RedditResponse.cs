using System.Text.Json.Serialization;

namespace Models.Reddit
{
    public class RedditResponse
    {
        [JsonPropertyName("kind")]
        public string Kind { get; set; }

        [JsonPropertyName("data")]
        public RedditData RedditData { get; set; }
    }
}