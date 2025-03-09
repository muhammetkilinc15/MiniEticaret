namespace ApiGateway.Models
{
    public sealed class User
    {
        public Guid Id { get; set; }

        public User()
        {
            Id = Guid.NewGuid();
        }
        public string UserName { get; set; } = default!;
        public string Password { get; set; } = default!;
    }
}
