using Microsoft.AspNetCore.Mvc;
using WebAppBlogApi.Core.Entities;
using WebAppBlogApi.Application.IServices;

namespace WebAppBlogApi.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApplicationUserController(IApplicationUserService userService) : ControllerBase
    {
        private readonly IApplicationUserService _userService = userService;

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpGet("username/{username}")]
        public async Task<IActionResult> GetByUsername(string username)
        {
            var user = await _userService.GetByUsernameAsync(username);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAllAsync();
            return Ok(users);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] ApplicationUser user)
        {
            await _userService.AddAsync(user);
            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] ApplicationUser user)
        {
            var existingUser = await _userService.GetByIdAsync(id);
            if (existingUser == null)
            {
                return NotFound();
            }

            user.Id = id;
            await _userService.UpdateAsync(user);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var existingUser = await _userService.GetByIdAsync(id);
            if (existingUser == null)
            {
                return NotFound();
            }

            await _userService.DeleteAsync(id);
            return NoContent();
        }
    }
}
