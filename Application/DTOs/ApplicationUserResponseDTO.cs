using System.ComponentModel.DataAnnotations;
using WebAppBlogApi.Core.Entities;

namespace WebAppBlogApi.Application.DTOs
{
    public record ApplicationUserResponseDTO(
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
