using WebAppBlogApi.Core.Entities;

namespace WebAppBlogApi.Core.IRepository
{
    public interface ICategoryRepository
    {
        Task<Category> GetByIdAsync(string id);
        Task<IEnumerable<Category>> GetAllAsync();
        Task AddAsync(Category category);
        Task UpdateAsync(Category category);
        Task DeleteAsync(string id);
    }
}
