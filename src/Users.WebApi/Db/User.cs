namespace Users.WebApi.Db
{
    public class User
    {
        public long Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public long CustomerId { get; set; }
        public string CustomerName { get; set; }
    }
}
