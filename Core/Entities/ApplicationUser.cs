using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace WebAppBlogApi.Core.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; private set; }
        public string Bio { get; private set; } = "This is my bio";
        public string ProfilePicture { get; private set; } = "https://example.com/profile.jpg";
        public List<Post> Posts { get; private set; } = [];
        public List<Comment> Comments { get; private set; } = [];

        public ApplicationUser(string username, string email, string fullName)
        {
            if (string.IsNullOrEmpty(username)) throw new ArgumentException("Username is required");
            if (string.IsNullOrEmpty(email)) throw new ArgumentException("Email is required");
            if (string.IsNullOrEmpty(fullName)) throw new ArgumentException("Full Name is required");

            UserName = username;
            Email = email;
            FullName = fullName;
        }

        public void UpdateProfile(string fullName, string bio, string profilePicture)
        {
            if (string.IsNullOrEmpty(fullName)) throw new ArgumentException("Full Name is required");
            if (string.IsNullOrEmpty(bio)) throw new ArgumentException("Bio is required");
            if (string.IsNullOrEmpty(profilePicture)) throw new ArgumentException("Profile Picture is required");

            FullName = fullName;
            Bio = bio;
            ProfilePicture = profilePicture;
        }
    }
}