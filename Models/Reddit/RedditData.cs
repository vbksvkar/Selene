using System.Text.Json.Serialization;

namespace Models.Reddit
{
    public class RedditData
    {
        [JsonPropertyName("children")]
        public List<RedditPostChild> Children { get; set; }

        [JsonPropertyName("dist")]
        public int Dist { get; set; }
    }
}