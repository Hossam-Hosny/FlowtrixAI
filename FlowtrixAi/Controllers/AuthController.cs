using FlowtrixAI.Application.JWT.Interface;
using FlowtrixAI.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FlowtrixAI.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(UserManager<AppUser> _userManager, IJwtService _jwtService) : ControllerBase
    {

        [HttpPost("register")]
        public async Task<IActionResult> Register(string userName, string password)
        {
            var user = new AppUser { UserName = userName,Name=userName.ToUpper() };

            var result = await _userManager.CreateAsync(user, password);


            if (!result.Succeeded)
                return BadRequest(result.Errors);

            await _userManager.AddToRoleAsync(user, "Engineer");

            return Ok(new { Message = "User registered successfully" });
        }

        [HttpPost("register-admin")]
        public async Task<IActionResult> RegisterAdmin(string userName, string password)
        {
            var user = new AppUser { UserName = userName,Name=userName.ToUpper() };

            var result = await _userManager.CreateAsync(user, password);


            if (!result.Succeeded)
                return BadRequest(result.Errors);

            await _userManager.AddToRoleAsync(user, "Admin");

            return Ok(new { Message = "User registered successfully" });
        }



        [HttpPost("login")]
        public async Task<IActionResult> Login(string userName, string password)
        {

            var user = await _userManager.FindByNameAsync(userName);

            if (user == null || !await _userManager.CheckPasswordAsync(user, password))
                return Unauthorized(new { Message = "Invalid username or password" });

            var token = await _jwtService.GenerateTokenAsync(user);
            return Ok(new { Token = token });
        }
    }
}
