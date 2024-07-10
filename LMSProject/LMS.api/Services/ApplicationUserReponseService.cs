using LMS.api.Data;
using LMS.api.Model;
using LMS.api.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LMS.api.Services
{
    public class ApplicationUserReponseService : IApplicationUserReponseService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public ApplicationUserReponseService(
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<List<ApplicationUser>> GetAsync(CancellationToken cancellation = default)
        {
            try
            {
                cancellation.ThrowIfCancellationRequested();
                return await _userManager.Users.ToListAsync(cancellation);
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while trying to get all users.", ex);
            }
        }

        public async Task<ApplicationUser?> GetAsync(string id, CancellationToken cancellation = default)
        {
            try
            {
                cancellation.ThrowIfCancellationRequested();
                return await _userManager.FindByIdAsync(id);
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while trying to get the user by id.", ex);
            }
        }

        public async Task AddAsync(ApplicationUser entity, CancellationToken cancellation = default)
        {
            try
            {
                cancellation.ThrowIfCancellationRequested();
                await _userManager.CreateAsync(entity);
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while trying to add the user.", ex);
            }
        }

        public async Task UpdateAsync(ApplicationUser entity, CancellationToken cancellation = default)
        {
            try
            {
                cancellation.ThrowIfCancellationRequested();
                await _userManager.UpdateAsync(entity);
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while trying to update the user.", ex);
            }
        }

        public async Task DeleteAsync(string id, CancellationToken cancellation = default)
        {
            try
            {
                cancellation.ThrowIfCancellationRequested();
                var user = await _userManager.FindByIdAsync(id);
                if (user == null)
                {
                    throw new InvalidOperationException($"The user with id '{id}' does not exist.");
                }

                await _userManager.DeleteAsync(user);
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while trying to delete the user.", ex);
            }
        }

        public async Task<ApplicationUser?> GetUserByEmail(string email, CancellationToken cancellation = default)
        {
            try
            {
                cancellation.ThrowIfCancellationRequested();
                return await _userManager.FindByEmailAsync(email);
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while trying to get the user by email.", ex);
            }
        }

        public async Task<ApplicationUser?> GetUserById(string id, CancellationToken cancellation = default)
        {
            try
            {
                cancellation.ThrowIfCancellationRequested();
                return await _userManager.FindByIdAsync(id);
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while trying to get the user by id.", ex);
            }
        }

        public async Task<ApplicationUser?> GetUserByUserName(string userName, CancellationToken cancellation = default)
        {
            try
            {
                cancellation.ThrowIfCancellationRequested();
                return await _userManager.FindByNameAsync(userName);
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while trying to get the user by username.", ex);
            }
        }

        public async Task<IEnumerable<ApplicationUser>> GetUsersByRole(string roleName, CancellationToken cancellation = default)
        {
            try
            {
                cancellation.ThrowIfCancellationRequested();
                var role = await _roleManager.FindByNameAsync(roleName);
                if (role == null)
                {
                    throw new InvalidOperationException($"The role '{roleName}' does not exist.");
                }

                return await _userManager.GetUsersInRoleAsync(roleName);
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while trying to get the users by role.", ex);
            }
        }

        public async Task<ApplicationUser> CreateUser(ApplicationUser user, string password, CancellationToken cancellation = default)
        {
            try
            {
                cancellation.ThrowIfCancellationRequested();
                var result = await _userManager.CreateAsync(user, password);
                if (!result.Succeeded)
                {
                    throw new InvalidOperationException($"An error occurred while trying to create the user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }

                return user;
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while trying to create the user.", ex);
            }
        }

    }
}