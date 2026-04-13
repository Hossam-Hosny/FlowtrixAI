using FlowtrixAI.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FlowtrixAI.Infrastructure.Context;

internal class AppDbContext (DbContextOptions<AppDbContext> options)
            : IdentityDbContext<AppUser, IdentityRole<int>, int>(options)
{
    
    public DbSet<Product> Products { get; set; }
    public DbSet<BillOfMaterial> BoMs { get; set; }
    public DbSet<Process> Processes { get; set; }
    public DbSet<ProductionRecord> ProductionRecords { get; set; }
    public DbSet<QualityCheck> QualityChecks { get; set; }
    public DbSet<Report> Reports { get; set; }
    public DbSet<InventoryItem> Inventory { get; set; }
    public DbSet<ProductionOrder> ProductionOrders { get; set; }








    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);


        modelBuilder.Entity<ProductionRecord>()
        .HasOne(p => p.Engineer)
        .WithMany(u => u.ProductionRecords)
        .HasForeignKey(p => p.EngineerId)
        .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<QualityCheck>()
            .HasOne(q => q.CheckedBy)
            .WithMany(u => u.QualityChecks)
            .HasForeignKey(q => q.CheckedById)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Report>()
            .HasOne(r => r.GeneratedBy)
            .WithMany(u => u.Reports)
            .HasForeignKey(r => r.GeneratedById)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Ignore<IdentityPasskeyData>();


    }

}
