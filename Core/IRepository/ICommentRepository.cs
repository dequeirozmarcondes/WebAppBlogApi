using WebAppBlogApi.Core.Entities;

namespace WebAppBlogApi.Core.IRepository
{
    public interface ICommentRepository
    {
        Task<Comment> GetByIdAsync(string id);
        Task<IEnumerable<Comment>> GetAllAsync();
        Task<IEnumerable<Comment>> GetByPostIdAsync(string postId);
        Task AddAsync(Comment comment);
        Task UpdateAsync(Comment comment);
        Task DeleteAsync(string id);
    }
}
