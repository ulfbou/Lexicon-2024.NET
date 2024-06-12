namespace MessengerAPI.Authentication.Models
{
    public interface IEntity
    {
        public int Id { get; set; }

        public string SearchableContent { get; }
    }
}