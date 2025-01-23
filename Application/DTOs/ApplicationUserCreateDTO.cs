using System.ComponentModel.DataAnnotations;

namespace WebAppBlogApi.Application.DTOs
{
    public record ApplicationUserCreateDTO(
        [Required]
        [StringLength(100, MinimumLength = 3)]
        string FullName,

        [Required]
        [StringLength(100, MinimumLength = 3)]
        string UserName,

        [Required]
        [EmailAddress]
        string Email,

        [Required]
        [StringLength(100, MinimumLength = 6)]
        string Password
    );

}
