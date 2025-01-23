using Raven.Client.Documents.Session;
using WebAppBlogApi.Core.Entities;
using WebAppBlogApi.Core.IRepository;
using System.Collections.Generic;
using System.Threading.Tasks;
using Raven.Client.Documents;

namespace WebAppBlogApi.Infrastructure.Data.Repository
{
    public class ApplicationUserRepository(IAsyncDocumentSession session) : IApplicationUserRepository
    {
        private readonly IAsyncDocumentSession _session = session;

        public async Task<ApplicationUser> GetByIdAsync(string id)
        {
            return await _session.LoadAsync<ApplicationUser>(id);
        }

        public async Task<ApplicationUser> GetByUsernameAsync(string username)
        {
            return await _session.Query<ApplicationUser>().FirstOrDefaultAsync(u => u.UserName == username);
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllAsync()
        {
            return await _session.Query<ApplicationUser>().ToListAsync();
        }

        public async Task AddAsync(ApplicationUser user)
        {
            await _session.StoreAsync(user);
            await _session.SaveChangesAsync();
        }

        public async Task UpdateAsync(ApplicationUser user)
        {
            var existingUser = await GetByIdAsync(user.Id);
            if (existingUser != null)
            {
                existingUser.UpdateProfile(user.FullName, user.Bio, user.ProfilePicture);
                await _session.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(string id)
        {
            _session.Delete(id);
            await _session.SaveChangesAsync();
        }
    }
}
