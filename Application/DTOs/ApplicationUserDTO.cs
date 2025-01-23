using System.ComponentModel.DataAnnotations;

namespace WebAppBlogApi.Application.DTOs
{
    public record ApplicationUserDTO(
        [Required]
        [StringLength(100, MinimumLength = 3)]
        string Name,

        [Required]
        [StringLength(100, MinimumLength = 3)]
        string UserName,

        [Required]
        [EmailAddress]
        string Email
    );

}
