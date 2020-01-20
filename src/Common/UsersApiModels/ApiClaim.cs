using System.Text.Json.Serialization;

namespace Common.UsersApiModels
{
    public class ApiClaim
    {
        [JsonPropertyName("claimType")]
        public string ClaimType { get; set; }

        [JsonPropertyName("claimValue")]
        public string ClaimValue { get; set; }

        [JsonPropertyName("claimValueType")]
        public string ClaimValueType { get; set; }
    }
}
