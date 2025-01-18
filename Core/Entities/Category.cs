namespace WebAppBlogApi.Core.Entities
{
    public class Category
    {
        public string? Id { get; private set; }
        public string Name { get; private set; }
        public List<Post> Posts { get; private set; } = [];

        public Category(string name)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentException("Name is required");

            Name = name;
            Posts = [];
        }

        public void AddPost(Post post)
        {
            ArgumentNullException.ThrowIfNull(post);
            Posts.Add(post);
        }
    }
}
