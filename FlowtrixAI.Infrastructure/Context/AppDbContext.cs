using FlowtrixAI.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FlowtrixAI.Infrastructure.Context;

internal class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<AppUser> Users { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<BillOfMaterial> BoMs { get; set; }
    public DbSet<Process> Processes { get; set; }
    public DbSet<ProductionRecord> ProductionRecords { get; set; }
    public DbSet<QualityCheck> QualityChecks { get; set; }
    public DbSet<Report> Reports { get; set; }
    public DbSet<InventoryItem> Inventory { get; set; }

}
