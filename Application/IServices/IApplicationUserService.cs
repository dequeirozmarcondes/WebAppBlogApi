using WebAppBlogApi.Core.Entities;

namespace WebAppBlogApi.Application.IServices
{
    public interface IApplicationUserService
    {
        Task<ApplicationUser> GetByIdAsync(string id);
        Task<ApplicationUser> GetByUsernameAsync(string username);
        Task<IEnumerable<ApplicationUser>> GetAllAsync();
        Task AddAsync(ApplicationUser user);
        Task UpdateAsync(ApplicationUser user);
        Task DeleteAsync(string id);
    }
}
