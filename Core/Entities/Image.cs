namespace WebAppBlogApi.Core.Entities
{
    public class Image
    {
        public string? Id { get; private set; }
        public string Url { get; private set; }
        public string? Caption { get; private set; }
        public string PostId { get; private set; } // Foreign Key
        public Post Post { get; private set; } // Navigation Property

        public Image(string url, string postId, Post post, string? caption = null)
        {
            if (string.IsNullOrEmpty(url)) throw new ArgumentException("URL is required");
            ArgumentNullException.ThrowIfNull(post);

            Url = url;
            Caption = caption;
            PostId = postId;
            Post = post;
        }

        public void UpdateImage(string url, string? caption)
        {
            if (string.IsNullOrEmpty(url)) throw new ArgumentException("URL is required");

            Url = url;
            Caption = caption;
        }
    }
}
