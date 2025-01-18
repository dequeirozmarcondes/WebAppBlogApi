namespace WebAppBlogApi.Core.Entities
{
    public class Tag
    {
        public string? Id { get; private set; }
        public string Name { get; private set; }
        public List<Post> Posts { get; private set; }

        public Tag(string name)
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
