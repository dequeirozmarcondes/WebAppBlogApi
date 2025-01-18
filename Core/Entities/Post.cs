namespace WebAppBlogApi.Core.Entities
{
    public class Post
    {
        public string? Id { get; private set; }
        public string Title { get; private set; }
        public string Content { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }
        public string UserId { get; private set; } // Foreign Key
        public ApplicationUser User { get; private set; } // Navigation Property
        public string? CategoryId { get; private set; } // Foreign Key
        public Category? Category { get; private set; } // Navigation Property

        public List<Image> Images { get; private set; }
        public List<Comment> Comments { get; private set; }
        public List<Tag> Tags { get; private set; }
        public int LikeCount { get; private set; }
        public Post(string title, string content, string userId, ApplicationUser user)
        {
            if (string.IsNullOrEmpty(title)) throw new ArgumentException("Title is required");
            if (string.IsNullOrEmpty(content)) throw new ArgumentException("Content is required");
            ArgumentNullException.ThrowIfNull(user);

            Title = title;
            Content = content;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
            UserId = userId;
            User = user;
            Images = new List<Image>(); // Inicializando a lista corretamente
            Comments = new List<Comment>(); // Inicializando a lista corretamente
            Tags = new List<Tag>(); // Inicializando a lista corretamente
        }

        public void UpdatePost(string title, string content)
        {
            if (string.IsNullOrEmpty(title)) throw new ArgumentException("Title is required");
            if (string.IsNullOrEmpty(content)) throw new ArgumentException("Content is required");

            Title = title;
            Content = content;
            UpdatedAt = DateTime.UtcNow;
        }

        public void AddImage(Image image)
        {
            ArgumentNullException.ThrowIfNull(image);
            Images.Add(image);
        }

        public void AddLike()
        {
            LikeCount++;
        }

        public void AddComment(Comment comment)
        {
            ArgumentNullException.ThrowIfNull(comment);
            Comments.Add(comment);
        }

        public void AddTag(Tag tag)
        {
            ArgumentNullException.ThrowIfNull(tag);
            Tags.Add(tag);
        }

        public void SetCategory(Category category)
        {
            ArgumentNullException.ThrowIfNull(category);
            Category = category;
            CategoryId = category.Id;
        }
    }
}
