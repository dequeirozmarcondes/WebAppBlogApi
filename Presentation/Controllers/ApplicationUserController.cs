using Microsoft.AspNetCore.Mvc;
using WebAppBlogApi.Core.Entities;
using WebAppBlogApi.Application.IServices;
using WebAppBlogApi.Application.DTOs;
using Microsoft.AspNetCore.Identity;

namespace WebAppBlogApi.Presentation.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class ApplicationUserController(IApplicationUserService userService, UserManager<ApplicationUser> userManager) : ControllerBase
    {
        private readonly IApplicationUserService _userService = userService;
        private readonly UserManager<ApplicationUser> _userManager = userManager;

        [HttpGet("getById/{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("ID is required.");
            }

            try
            {
                var user = await _userService.GetByIdAsync(id);
                if (user == null)
                {
                    return NotFound();
                }

                if (string.IsNullOrEmpty(user.UserName) || string.IsNullOrEmpty(user.Email))
                {
                    return StatusCode(500, "User data is incomplete.");
                }

                var userDto = new ApplicationUserResponseDTO(
                    user.Id,
                    user.FullName,
                    user.UserName,
                    user.Email,
                    user.Bio,
                    user.ProfilePicture,
                    user.Posts,
                    user.Comments
                );
                return Ok(userDto);
            }
            catch (Exception ex)
            {
                // Log the exception and return an appropriate error response
                // LogException(ex);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("getByUsername/{username}")]
        public async Task<IActionResult> GetByUsername(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return BadRequest("Username is required.");
            }

            try
            {
                var user = await _userService.GetByUsernameAsync(username);
                if (user == null)
                {
                    return NotFound();
                }

                if (string.IsNullOrEmpty(user.UserName) || string.IsNullOrEmpty(user.Email))
                {
                    return StatusCode(500, "User data is incomplete.");
                }

                var userDto = new ApplicationUserResponseDTO(
                    user.Id,
                    user.FullName,
                    user.UserName,
                    user.Email,
                    user.Bio,
                    user.ProfilePicture,
                    user.Posts,
                    user.Comments
                );
                return Ok(userDto);
            }
            catch (Exception ex)
            {
                // Log the exception and return an appropriate error response
                // LogException(ex);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var users = await _userService.GetAllAsync();
                var userDtos = users.Select(user =>
                {
                    if (string.IsNullOrEmpty(user.UserName) || string.IsNullOrEmpty(user.Email))
                    {
                        // Skip users with incomplete data
                        return null;
                    }

                    return new ApplicationUserResponseDTO(
                        user.Id,
                        user.FullName,
                        user.UserName,
                        user.Email,
                        user.Bio,
                        user.ProfilePicture,
                        user.Posts,
                        user.Comments
                    );
                }).Where(dto => dto != null).ToList();

                return Ok(userDtos);
            }
            catch (Exception ex)
            {
                // Log the exception and return an appropriate error response
                // LogException(ex);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("addUser")]
        public async Task<IActionResult> Add([FromBody] ApplicationUserCreateDTO userDto)
        {
            if (userDto == null)
            {
                return BadRequest("User data is required.");
            }

            try
            {
                var user = new ApplicationUser(userDto.UserName, userDto.Email, userDto.FullName);
                var result = await _userManager.CreateAsync(user, userDto.Password);

                if (!result.Succeeded)
                {
                    return BadRequest(result.Errors);
                }

                if (string.IsNullOrEmpty(user.UserName) || string.IsNullOrEmpty(user.Email))
                {
                    return StatusCode(500, "User ID is missing.");
                }

                var responseDto = new ApplicationUserResponseDTO(
                    user.Id,
                    user.FullName,
                    user.UserName,
                    user.Email,
                    user.Bio,
                    user.ProfilePicture,
                    user.Posts,
                    user.Comments
                );
                return CreatedAtAction(nameof(GetById), new { id = user.Id }, responseDto);
            }
            catch (Exception ex)
            {
                // Log the exception and return an appropriate error response
                // LogException(ex);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("updateUser/{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] ApplicationUserUpdateDTO userDto)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("ID is required.");
            }

            if (userDto == null)
            {
                return BadRequest("User data is required.");
            }

            try
            {
                var existingUser = await _userService.GetByIdAsync(id);
                if (existingUser == null)
                {
                    return NotFound();
                }

                existingUser.UpdateProfile(userDto.FullName, userDto.Bio, userDto.ProfilePicture);
                await _userService.UpdateAsync(existingUser);
                return NoContent();
            }
            catch (Exception ex)
            {
                // Log the exception and return an appropriate error response
                // LogException(ex);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("deleteUser/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("ID is required.");
            }

            try
            {
                var existingUser = await _userService.GetByIdAsync(id);
                if (existingUser == null)
                {
                    return NotFound();
                }

                await _userService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                // Log the exception and return an appropriate error response
                // LogException(ex);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}