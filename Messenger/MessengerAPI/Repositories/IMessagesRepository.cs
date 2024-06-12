using MessengerAPI.Models;

namespace MessengerAPI.Repositories
{
    /// <summary>
    /// Represents a generic interface for a repository that provides basic CRUD operations for entities of type <see cref="Message"/>.
    /// </summary>
    public interface IMessagesRepository : IRepository<Message>
    {
    }
}