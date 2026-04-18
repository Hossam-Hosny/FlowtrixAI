using FlowtrixAI.Application.JWT.Interface;
using FlowtrixAI.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FlowtrixAI.Application.JWT.Services;

internal class JwtService(IConfiguration _config, UserManager<AppUser> _userManager) : IJwtService
{
    public async Task<string> GenerateTokenAsync(AppUser user)
    {

        var Claims = new List<Claim>
        {
          new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
          new Claim(ClaimTypes.Name, user.Name),
          new Claim("UserName", user.UserName)
        };

        var roles = await _userManager.GetRolesAsync(user);
        foreach (var role in roles)
        {
            Claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(

            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: Claims,
            expires: DateTime.Now.AddDays(7),
            signingCredentials: creds

            );


        return new JwtSecurityTokenHandler().WriteToken(token);

    }
}
