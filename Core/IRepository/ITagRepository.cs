using WebAppBlogApi.Core.Entities;

namespace WebAppBlogApi.Core.IRepository
{
    public interface ITagRepository
    {
        Task<Tag> GetByIdAsync(string id);
        Task<IEnumerable<Tag>> GetAllAsync();
        Task<IEnumerable<Tag>> GetByPostIdAsync(string postId);
        Task AddAsync(Tag tag); Task UpdateAsync(Tag tag);
        Task DeleteAsync(string id);
    }
}
