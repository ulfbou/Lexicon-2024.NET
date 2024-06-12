using MessengerAPI.Authentication.Models;

namespace MessengerAPI.Authentication.Repositories
{
    /// <summary>
    /// Represents a generic interface for a repository that provides basic CRUD operations for entities of type <see cref="User"/>.
    /// </summary>
    public interface IUsersRepository : IRepository<User>
    {
    }
}
