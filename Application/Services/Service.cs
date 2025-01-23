using WebAppBlogApi.Core.Entities;
using WebAppBlogApi.Core.IRepository;
using WebAppBlogApi.Application.IServices;

namespace WebAppBlogApi.Application.Services
{
    public class ApplicationUserService(IApplicationUserRepository userRepository) : IApplicationUserService
    {
        private readonly IApplicationUserRepository _userRepository = userRepository;

        public async Task<ApplicationUser> GetByIdAsync(string id)
        {
            try
            {
                return await _userRepository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                // Log the exception and rethrow or handle it
                // LogException(ex);
                throw new ApplicationException($"Error fetching user by Id: {id}", ex);
            }
        }

        public async Task<ApplicationUser> GetByUsernameAsync(string username)
        {
            try
            {
                return await _userRepository.GetByUsernameAsync(username);
            }
            catch (Exception ex)
            {
                // Log the exception and rethrow or handle it
                // LogException(ex);
                throw new ApplicationException($"Error fetching user by Username: {username}", ex);
            }
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllAsync()
        {
            try
            {
                return await _userRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                // Log the exception and rethrow or handle it
                // LogException(ex);
                throw new ApplicationException("Error fetching all users", ex);
            }
        }

        public async Task AddAsync(ApplicationUser user)
        {
            try
            {
                await _userRepository.AddAsync(user);
            }
            catch (Exception ex)
            {
                // Log the exception and rethrow or handle it
                // LogException(ex);
                throw new ApplicationException("Error adding user", ex);
            }
        }

        public async Task UpdateAsync(ApplicationUser user)
        {
            try
            {
                await _userRepository.UpdateAsync(user);
            }
            catch (Exception ex)
            {
                // Log the exception and rethrow or handle it
                // LogException(ex);
                throw new ApplicationException($"Error updating user with Id: {user.Id}", ex);
            }
        }

        public async Task DeleteAsync(string id)
        {
            try
            {
                await _userRepository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                // Log the exception and rethrow or handle it
                // LogException(ex);
                throw new ApplicationException($"Error deleting user with Id: {id}", ex);
            }
        }
    }
}