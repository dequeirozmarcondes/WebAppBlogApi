using WebAppBlogApi.Core.Entities;

namespace WebAppBlogApi.Core.IRepository
{
    public interface IImageRepository
    {
        Task<Image> GetByIdAsync(string id);
        Task<IEnumerable<Image>> GetAllAsync();
        Task<IEnumerable<Image>> GetByPostIdAsync(string postId);
        Task AddAsync(Image image); Task UpdateAsync(Image image);
        Task DeleteAsync(string id);
    }
}
