﻿using MessengerAPI.Data;
using MessengerAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace MessengerAPI.Repositories
{
    /// <summary>
    /// Represents a class for a repository that implements <see cref="IRepositoryBase{Message}"/>.
    /// </summary>
    public class UsersRepository(DbSet<User> repository)
        : Repository<User>(repository), IUsersRepository
    {
    }
}
