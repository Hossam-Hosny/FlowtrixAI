using FlowtrixAI.Application.Inventory.Interface;
using FlowtrixAI.Application.Inventory.Services;
using FlowtrixAI.Application.JWT.Interface;
using FlowtrixAI.Application.JWT.Services;
using FlowtrixAI.Application.Production.Interface;
using FlowtrixAI.Application.Production.Services;
using FlowtrixAI.Application.Quality.Interface;
using FlowtrixAI.Application.Quality.Services;
using FlowtrixAI.Application.Reports.Interface;
using FlowtrixAI.Application.Reports.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FlowtrixAI.Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddApplication(this IServiceCollection services)
        {
            


            services.AddScoped<IInventoryService,InventoryService>();
            services.AddScoped<IProductionService, ProductionService>();
            services.AddScoped<IQualityService,  QualityService>();
            services.AddScoped<IReportService, ReportService>();

            services.AddScoped<IJwtService, JwtService>();



           

        }
    }
}
