using FlowtrixAI.Domain.Constants;
using FlowtrixAI.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FlowtrixAI.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = UserRoles.Admin)]
    public class AdminController(UserManager<AppUser> _userManager) : ControllerBase
    {
        [HttpPost("add-engineer")]
        public async Task<IActionResult> AddEngineer([FromBody] AddEngineerRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Name) || string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest(new { Message = "Name and Password are required." });
            }

            // التحقق من أن الاسم غير موجود مسبقاً (تجاهل حالة الأحرف)
            bool nameExists = await Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions.AnyAsync(_userManager.Users, u => u.Name.ToLower() == request.Name.ToLower());
            if (nameExists)
            {
                return BadRequest(new { Message = "هذا الاسم موجود بالفعل لمستخدم آخر، يرجى اختيار اسم مختلف" });
            }

            // Create a unique username from the name
            var baseUserName = request.Name.Replace(" ", "").ToLower();
            var userName = baseUserName;
            int counter = 1;

            while (await _userManager.FindByNameAsync(userName) != null)
            {
                userName = $"{baseUserName}{counter++}";
            }

            var user = new AppUser
            {
                UserName = userName,
                Name = request.Name,
                Email = $"{userName}@flowtrix.ai", // Basic email generation
                CreatedAt = DateTime.UtcNow,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                return BadRequest(new { Message = "Failed to create user", Errors = result.Errors });
            }

            // Assign the Engineer role
            var roleResult = await _userManager.AddToRoleAsync(user, UserRoles.Engineer);

            if (!roleResult.Succeeded)
            {
                return BadRequest(new { Message = "User created but failed to assign role", Errors = roleResult.Errors });
            }

            return Ok(new
            {
                Message = "Engineer added successfully",
                Data = new
                {
                    user.Id,
                    user.UserName,
                    user.Name,
                    Role = UserRoles.Engineer
                }
            });
        }
    }

    public class AddEngineerRequest
    {
        public required string Name { get; set; }
        public required string Password { get; set; }
    }
}
