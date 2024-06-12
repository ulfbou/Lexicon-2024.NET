using MessengerAPI.Authentication.Data;
using MessengerAPI.Authentication.Models;
using Microsoft.EntityFrameworkCore;

namespace MessengerAPI.Authentication.Repositories
{
    /// <summary>
    /// Represents a class for a repository that implements <see cref="IRepositoryBase{Message}"/>.
    /// </summary>
    public class MessagesRepository(DbSet<Message> repository)
        : Repository<Message>(repository), IMessagesRepository
    {
    }
}
