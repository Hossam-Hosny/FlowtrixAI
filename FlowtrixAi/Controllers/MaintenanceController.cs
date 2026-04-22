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
                // 1. تعطيل جميع القيود (Constraints) مؤقتاً لتجنب مشاكل الـ Foreign Keys
                await _context.Database.ExecuteSqlRawAsync("EXEC sp_MSforeachtable 'ALTER TABLE ? NOCHECK CONSTRAINT ALL'");

                // 2. مسح البيانات من جميع الجداول
                await _context.Database.ExecuteSqlRawAsync("DELETE FROM [AiChatHistories]");
                await _context.Database.ExecuteSqlRawAsync("DELETE FROM [QualityChecks]");
                await _context.Database.ExecuteSqlRawAsync("DELETE FROM [ProductionRecords]");
                await _context.Database.ExecuteSqlRawAsync("DELETE FROM [ProductionOrders]");
                await _context.Database.ExecuteSqlRawAsync("DELETE FROM [Reports]");
                await _context.Database.ExecuteSqlRawAsync("DELETE FROM [Exports]");
                await _context.Database.ExecuteSqlRawAsync("DELETE FROM [Processes]");
                await _context.Database.ExecuteSqlRawAsync("DELETE FROM [Inventory]");
                await _context.Database.ExecuteSqlRawAsync("DELETE FROM [BoMs]");
                await _context.Database.ExecuteSqlRawAsync("DELETE FROM [Products]");

                // 3. إعادة تصفير الـ Identifiers للجداول الأساسية فقط (التي تمتلك Identity)
                var tablesToReseed = new[] { "Inventory", "Products", "ProductionOrders", "ProductionRecords", "QualityChecks", "Exports", "AiChatHistories" };
                foreach (var table in tablesToReseed)
                {
                    try {
                        await _context.Database.ExecuteSqlRawAsync($"DBCC CHECKIDENT ('[{table}]', RESEED, 0)");
                    } catch { /* نادراً ما تفشل هنا بعد التحقق من اسم الجدول */ }
                }

                // 4. إعادة تفعيل القيود (Constraints)
                await _context.Database.ExecuteSqlRawAsync("EXEC sp_MSforeachtable 'ALTER TABLE ? WITH CHECK CHECK CONSTRAINT ALL'");

                return Ok(new { Message = "تم تصفير جميع بيانات النظام بنجاح. تم الحفاظ على حسابات المستخدمين والأدوار." });
            }
            catch (Exception ex)
            {
                // في حالة الفشل، نحاول إعادة تفعيل القيود برضه لضمان سلامة الداتا
                try { await _context.Database.ExecuteSqlRawAsync("EXEC sp_MSforeachtable 'ALTER TABLE ? WITH CHECK CHECK CONSTRAINT ALL'"); } catch {}
                
                return StatusCode(500, new { Message = "حدث خطأ أثناء تصفير العدادات، لكن البيانات غالباً تم مسحها.", Error = ex.Message });
            }
        }
    }

    public class ResetDatabaseRequest
    {
        public string Password { get; set; } = string.Empty;
    }
}
