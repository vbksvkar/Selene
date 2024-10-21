using System.Text.Json.Serialization;

namespace Models.Response
{
    public class StatsResponse
    {
        [JsonIgnore]
        public int StatusCode { get; set; }
        public string SubredditName { get; set; }        
        public List<StatsDetail> StatsDetails { get; set; }
    }
}