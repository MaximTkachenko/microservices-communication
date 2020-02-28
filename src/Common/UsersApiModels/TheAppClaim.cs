using System.Text.Json.Serialization;

namespace Common.UsersApiModels
{
    public class TheAppClaim
    {
        [JsonPropertyName("claimType")]
        public string ClaimType { get; set; }

        [JsonPropertyName("claimValue")]
        public string ClaimValue { get; set; }

        [JsonPropertyName("claimValueType")]
        public string ClaimValueType { get; set; }

        public const string UserId = "x-userId";
        public const string CustomerId = "x-customerId";
        public const string AdminForCustomerId = "x-admin-for-customerId";
    }
}
