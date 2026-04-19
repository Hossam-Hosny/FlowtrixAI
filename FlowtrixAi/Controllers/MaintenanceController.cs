using FlowtrixAI.Domain.Constants;
using FlowtrixAI.Domain.Entities;
using FlowtrixAI.Infrastructure.Context;
using FlowtrixAI.Infrastructure.Seeders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FlowtrixAI.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = UserRoles.Admin)]
    public class MaintenanceController(
        AppDbContext _context,
        UserManager<AppUser> _userManager,
        RoleManager<IdentityRole<int>> _roleManager) : ControllerBase
    {
        [HttpPost("reset-database")]
        public async Task<IActionResult> ResetDatabase([FromBody] ResetDatabaseRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest(new { Message = "كلمة المرور مطلوبة لإتمام هذه العملية الحساسة" });
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return Unauthorized();

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!isPasswordValid)
            {
                return BadRequest(new { Message = "كلمة المرور غير صحيحة. لا يمكن تصفير النظام إلا بكلمة مرور الأدمن." });
            }

            try
            {
                // إغلاق الاتصالات النشطة إذا لزم الأمر (بيعتمد على نوع قاعدة البيانات)
                // في حالة SQLite أو SQL Server Express، EnsureDeleted عادة ما تنجح إذا لم تكن هناك أقفال.
                
                await _context.Database.EnsureDeletedAsync();
                await _context.Database.MigrateAsync();
                
                // إعادة بذر البيانات الأساسية
                await RoleSeeder.SeedRolesAsync(_roleManager);
                await UserSeeder.SeedAdminUserAsync(_userManager);

                return Ok(new { Message = "تم تصفير النظام وإعادة التهيئة بنجاح. سيتم توجيهك لصفحة تسجيل الدخول باستخدام الحساب الافتراضي." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "فشل في تصفير النظام. قد تكون قاعدة البيانات قيد الاستخدام حالياً.", Error = ex.Message });
            }
        }
    }

    public class ResetDatabaseRequest
    {
        public string Password { get; set; } = string.Empty;
    }
}
