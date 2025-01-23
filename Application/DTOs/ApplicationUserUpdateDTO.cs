using WebAppBlogApi.Core.Entities;

namespace WebAppBlogApi.Application.DTOs
{
    public record ApplicationUserUpdateDTO(
        string Id,
        string FullName,
        string UserName,
        string Email,
        string Bio,
        string ProfilePicture,
        List<Post> Posts,
        List<Comment> Comments
    );
}
