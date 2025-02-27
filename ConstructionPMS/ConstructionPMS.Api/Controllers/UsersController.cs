using Microsoft.AspNetCore.Authorization; // Import the authorization namespace
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ConstructionPMS.Services;
using ConstructionPMS.Domain.Entities;
using ConstructionPMS.Services.NotificationService;
using Nest;

namespace ConstructionPMS.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly INotificationService _notificationService;

        public UsersController(IUserService userService, INotificationService notificationService)
        {
            _userService = userService;
            _notificationService = notificationService;
        }

        // GET: api/users
        [Authorize] // Require authentication
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        // GET: api/users/{id}
        [Authorize] // Require authentication
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUserById(Guid id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        // POST: api/users
        [Authorize(Roles = "Admin")] // Require authentication and Admin role
        [HttpPost]
        public async Task<ActionResult<User>> CreateUser([FromBody] User user)
        {
            var createdUser = await _userService.CreateUserAsync(user);

            // Send a notification after creating a user
            await _notificationService.SendNotificationAsync($"User  '{createdUser.Username}' has been created.");

            return CreatedAtAction(nameof(GetUserById), new { id = createdUser.Id }, createdUser);
        }

        // PUT: api/users/{id}
        [Authorize(Roles = "Admin")] // Require authentication and Admin role
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(Guid id, [FromBody] User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            await _userService.UpdateUserAsync(user);

            // Send a notification after updating a user
            await _notificationService.SendNotificationAsync($"User  '{user.Username}' has been updated.");

            return NoContent();
        }

        // DELETE: api/users/{id}
        [Authorize(Roles = "Admin")] // Require authentication and Admin role
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            await _userService.DeleteUserAsync(id);

            // Send a notification after deleting a user
            await _notificationService.SendNotificationAsync($"User  '{user.Username}' has been deleted.");

            return NoContent();
        }
    }
}