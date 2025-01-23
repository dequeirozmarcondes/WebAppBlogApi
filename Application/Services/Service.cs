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
            return await _userRepository.GetByIdAsync(id);
        }

        public async Task<ApplicationUser> GetByUsernameAsync(string username)
        {
            return await _userRepository.GetByUsernameAsync(username);
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllAsync()
        {
            return await _userRepository.GetAllAsync();
        }

        public async Task AddAsync(ApplicationUser user)
        {
            await _userRepository.AddAsync(user);
        }

        public async Task UpdateAsync(ApplicationUser user)
        {
            await _userRepository.UpdateAsync(user);
        }

        public async Task DeleteAsync(string id)
        {
            await _userRepository.DeleteAsync(id);
        }
    }
}
