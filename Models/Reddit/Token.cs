using System.Text.Json.Serialization;
using Models.Extensions;

namespace Models.Reddit
{
    public class Token
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }

        [JsonPropertyName("token_type")]
        public string TokenType { get; set; }

        [JsonPropertyName("expires_in")]
        public long ExpiresIn { get; set; }
        
        [JsonPropertyName("scope")]
        public string Scope { get; set; }

        public DateTime? ExpiryTime
        {
            get
            {
                return this.ExpiresIn.UtcFromEpochTime();
            }
        }
    }
}