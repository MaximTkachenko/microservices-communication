namespace Users.WebApi.Db
{
    public class TheAppClaim
    {
        public long Id { get; set; }

        public long UserId { get; set; }
        public User User { get; set; }

        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
        public string ClaimValueType { get; set; }
    }
}
