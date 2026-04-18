using FlowtrixAI.Domain.Entities;
using FlowtrixAI.Domain.Repositories;
using FlowtrixAI.Infrastructure.Context;
using FlowtrixAI.Infrastructure.Repositories;
using FlowtrixAI.Infrastructure.Seeders;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FlowtrixAI.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddInfrastructure(this IServiceCollection services , IConfiguration config)
        {
            var connectionString = config.GetConnectionString("LocalConnectionString");
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });

            services.AddIdentity<AppUser,IdentityRole<int>>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                // السماح بالحروف العربية والأرقام والمسافات وبعض الرموز الخاصة
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+ اأإآببتثجحخدذرزسشصضطظعغفقكلمنهويىيةئؤء";
            });


            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IInventoryRepository, InventoryRepository>();
            services.AddScoped<IBomRepository, BomRepository>();

            services.AddScoped<IReportRepository, ReportRepository>();
            services.AddScoped<IProcessRepository, ProcessRepository>();
            services.AddScoped<IProductionRecordRepository, ProductionRecordRepository>();
            services.AddScoped<IQualityCheckRepository, QualityCheckRepository>();

            services.AddScoped<IProductionOrderRepository, ProductionOrderRepository>();
            services.AddScoped<IExportRepository, ExportRepository>();
            services.AddScoped<IAiChatRepository, AiChatRepository>();




        }
    }
}
