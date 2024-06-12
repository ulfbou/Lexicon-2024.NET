namespace MessengerAPI.Authentication.Models
{
    public class User : IEntity
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string? FirstName { get; internal set; }
        public string? LastName { get; internal set; }
        public string Email { get; set; }
        public string? Phone { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime Updated { get; set; } = DateTime.Now;

        public string SearchableContent => $"UserName: `{Username}` FirstName: `{FirstName}` LastName: `{LastName}` Email: `{Email}` Phone: `{Phone}`";
        public ICollection<Message> Messages { get; set; } = new List<Message>();
    }
}
