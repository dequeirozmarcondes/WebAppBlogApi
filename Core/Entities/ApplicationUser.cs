using Microsoft.AspNetCore.Identity;

namespace WebAppBlogApi.Core.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; private set; }
        public string? Bio { get; private set; }
        public string? ProfilePicture { get; private set; }
        public List<Post> Posts { get; private set; }
        public List<Comment> Comments { get; private set; }

        public ApplicationUser(string username, string email, string fullName)
        {
            if (string.IsNullOrEmpty(username)) throw new ArgumentException("Username is required");
            if (string.IsNullOrEmpty(email)) throw new ArgumentException("Email is required");
            if (string.IsNullOrEmpty(fullName)) throw new ArgumentException("Full Name is required");

            UserName = username;
            Email = email;
            FullName = fullName;
            Posts = [];
            Comments = [];
        }

        public void UpdateProfile(string fullName, string bio, string profilePicture)
        {
            FullName = fullName;
            Bio = bio;
            ProfilePicture = profilePicture;
        }
    }
}
