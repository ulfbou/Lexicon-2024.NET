namespace MessengerAPI.Authentication.Models
{
    public class Message : IEntity
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public string Content { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.Now;

        public User Sender { get; set; }
        public User Receiver { get; set; }
        public string SearchableContent => $"From: {Sender.Username} To: {Receiver.Username} Sent: {Timestamp.ToString("yyyy-MM-dd HH-mm-ss")} {Content}";
    }
}
