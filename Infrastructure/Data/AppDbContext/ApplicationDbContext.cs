using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Raven.Client.Documents.Session;
using WebAppBlogApi.Core.Entities;

namespace WebAppBlogApi.Infrastructure
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        private readonly IAsyncDocumentSession _session;

        public ApplicationDbContext(IAsyncDocumentSession session)
        {
            _session = session ?? throw new ArgumentNullException(nameof(session));
        }

        public DbSet<Post> Posts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Tag> Tags { get; set; }

        // Override SaveChangesAsync to throw an exception
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException("Use RavenDB session SaveChangesAsync method instead.");
        }

        // Method to save changes using RavenDB session
        public async Task SaveChangesWithRavenDbAsync()
        {
            await _session.SaveChangesAsync();
        }
    }
}
