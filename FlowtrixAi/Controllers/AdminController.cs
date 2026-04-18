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
    

        [HttpGet("users")]
        public async Task<IActionResult> GetUsers()
        {
            var users = _userManager.Users.ToList();
            var userList = new List<object>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userList.Add(new
                {
                    user.Id,
                    user.UserName,
                    user.Name,
                    user.CreatedAt,
                    Role = roles.FirstOrDefault() ?? "No Role"
                });
            }

            return Ok(userList);
        }

        [HttpPut("update-user")]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserRequest request)
        {
            var user = await _userManager.FindByIdAsync(request.Id);
            if (user == null) return NotFound(new { Message = "المستخدم غير موجود" });

            // Update Name if provided
            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                // التحقق من فرادة الاسم الجديد لو كان مختلفاً
                if (user.Name.ToLower() != request.Name.ToLower())
                {
                    bool nameExists = await Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions.AnyAsync(_userManager.Users, u => u.Name.ToLower() == request.Name.ToLower());
                    if (nameExists) return BadRequest(new { Message = "هذا الاسم مستخدم بالفعل" });
                }
                user.Name = request.Name;
            }

            // Update Password if provided
            if (!string.IsNullOrWhiteSpace(request.Password))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var result = await _userManager.ResetPasswordAsync(user, token, request.Password);
                if (!result.Succeeded) return BadRequest(new { Message = "فشل في تغيير كلمة المرور", Errors = result.Errors });
            }

            // Update Role if provided
            if (!string.IsNullOrWhiteSpace(request.Role))
            {
                var currentRoles = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, currentRoles);
                await _userManager.AddToRoleAsync(user, request.Role);
            }

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded) return BadRequest(new { Message = "فشل في تحديث بيانات المستخدم" });

            return Ok(new { Message = "تم تحديث البيانات بنجاح" });
        }

        [HttpDelete("delete-user/{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound(new { Message = "المستخدم غير موجود" });

            // منع الأدمن من مسح نفسه (اختياري للأمان)
            var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (user.Id.ToString() == currentUserId) return BadRequest(new { Message = "لا يمكنك حذف حسابك الخاص من هنا" });

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded) return BadRequest(new { Message = "فشل في حذف المستخدم" });

            return Ok(new { Message = "تم حذف المستخدم بنجاح" });
        }
    }

    public class AddEngineerRequest
    {
        public required string Name { get; set; }
        public required string Password { get; set; }
    }

    public class UpdateUserRequest
    {
        public required string Id { get; set; }
        public string? Name { get; set; }
        public string? Password { get; set; }
        public string? Role { get; set; }
    }
}
