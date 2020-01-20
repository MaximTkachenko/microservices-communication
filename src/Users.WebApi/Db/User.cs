using System.Collections.Generic;

namespace Users.WebApi.Db
{
    public class User
    {
        private readonly List<TheAppClaim> _claims = new List<TheAppClaim>();

        public long Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public long CustomerId { get; set; }
        public string CustomerName { get; set; }

        public IEnumerable<TheAppClaim> Claims => _claims.AsReadOnly();
    }
}
