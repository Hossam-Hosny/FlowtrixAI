using FlowtrixAI.Application.JWT.Interface;
using FlowtrixAI.Domain.Constants;
using FlowtrixAI.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FlowtrixAI.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(UserManager<AppUser> _userManager, IJwtService _jwtService) : ControllerBase
    {
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            // البحث عن المستخدم مع تجاهل حالة الأحرف (كبيرة أو صغيرة)
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Name.ToLower() == request.Name.ToLower());

            if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
                return BadRequest(new { Message = "الاسم أو كلمة المرور غير صحيحة" });

            var token = await _jwtService.GenerateTokenAsync(user);
            return Ok(new { Token = token });
        }

        public class LoginRequest
        {
            public required string Name { get; set; }
            public required string Password { get; set; }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(string userName, string password)
        {
            var user = new AppUser { UserName = userName, Name = userName, CreatedAt = DateTime.UtcNow };

            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            await _userManager.AddToRoleAsync(user, UserRoles.Engineer);

            return Ok(new { Message = "تم تسجيل المستخدم بنجاح" });
        }

        [Authorize(Roles = UserRoles.Admin)]
        [HttpPost("register-admin")]
        public async Task<IActionResult> RegisterAdmin(string userName, string password)
        {
            var user = new AppUser { UserName = userName, Name = userName, CreatedAt = DateTime.UtcNow };

            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            await _userManager.AddToRoleAsync(user, UserRoles.Admin);

            return Ok(new { Message = "تم تسجيل المسؤول بنجاح" });
        }
    }
}
