using System.ComponentModel.DataAnnotations.Schema;

namespace WebAppBlogApi.Core.Entities
{
    public class Comment
    {
        public string? Id { get; private set; }
        public string Content { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }
        [ForeignKey("User")]
        public string UserId { get; private set; } // Foreign Key
        public ApplicationUser User { get; private set; } // Navigation Property
        [ForeignKey("Post")]
        public string PostId { get; private set; } // Foreign Key       
        public Post Post { get; private set; } // Navigation Property

        public Comment(string content, string userId, ApplicationUser user, string postId, Post post)
        {
            if (string.IsNullOrEmpty(content)) throw new ArgumentException("Content is required");
            ArgumentNullException.ThrowIfNull(user);
            ArgumentNullException.ThrowIfNull(post);

            Content = content;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
            UserId = userId;
            User = user;
            PostId = postId;
            Post = post;
        }

        public void UpdateComment(string content)
        {
            if (string.IsNullOrEmpty(content)) throw new ArgumentException("Content is required");

            Content = content;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
