using Raven.Client.Documents.Session;
using WebAppBlogApi.Core.Entities;
using WebAppBlogApi.Core.IRepository;
using Raven.Client.Documents;

namespace WebAppBlogApi.Infrastructure.Data.Repository
{
    public class ApplicationUserRepository : IApplicationUserRepository
    {
        private readonly IAsyncDocumentSession _session;

        public ApplicationUserRepository(IAsyncDocumentSession session)
        {
            _session = session;
        }

        public async Task<ApplicationUser> GetByIdAsync(string id)
        {
            try
            {
                return await _session.LoadAsync<ApplicationUser>(id);
            }
            catch (Exception ex)
            {
                // Log the exception and rethrow or return a default value
                // LogException(ex);
                throw new ApplicationException($"Error fetching user by Id: {id}", ex);
            }
        }

        public async Task<ApplicationUser> GetByUsernameAsync(string username)
        {
            try
            {
                return await _session.Query<ApplicationUser>().FirstOrDefaultAsync(u => u.UserName == username);
            }
            catch (Exception ex)
            {
                // Log the exception and rethrow or return a default value
                // LogException(ex);
                throw new ApplicationException($"Error fetching user by Username: {username}", ex);
            }
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllAsync()
        {
            try
            {
                return await _session.Query<ApplicationUser>().ToListAsync();
            }
            catch (Exception ex)
            {
                // Log the exception and rethrow or return a default value
                // LogException(ex);
                throw new ApplicationException("Error fetching all users", ex);
            }
        }

        public async Task AddAsync(ApplicationUser user)
        {
            try
            {
                await _session.StoreAsync(user);
                await _session.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Log the exception and rethrow or return a default value
                // LogException(ex);
                throw new ApplicationException("Error adding user", ex);
            }
        }

        public async Task UpdateAsync(ApplicationUser user)
        {
            try
            {
                var existingUser = await GetByIdAsync(user.Id);
                if (existingUser != null)
                {
                    existingUser.UpdateProfile(user.FullName, user.Bio, user.ProfilePicture);
                    await _session.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                // Log the exception and rethrow or return a default value
                // LogException(ex);
                throw new ApplicationException($"Error updating user with Id: {user.Id}", ex);
            }
        }

        public async Task DeleteAsync(string id)
        {
            try
            {
                _session.Delete(id);
                await _session.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Log the exception and rethrow or return a default value
                // LogException(ex);
                throw new ApplicationException($"Error deleting user with Id: {id}", ex);
            }
        }
    }
}