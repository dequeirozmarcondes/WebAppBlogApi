using WebAppBlogApi.Core.Entities;

namespace WebAppBlogApi.Core.IRepository
{
    public interface IPostRepository
    {
        Task<Post> GetByIdAsync(string id);
        Task<IEnumerable<Post>> GetAllAsync();
        Task<IEnumerable<Post>> GetByUserIdAsync(string userId);
        Task<IEnumerable<Post>> GetByCategoryIdAsync(string categoryId);
        Task AddAsync(Post post);
        Task UpdateAsync(Post post); Task DeleteAsync(string id);
    }
}
