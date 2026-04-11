using FlowtrixAI.Application.Inventory.Interface;
using FlowtrixAI.Application.Inventory.Services;
using Microsoft.Extensions.DependencyInjection;

namespace FlowtrixAI.Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddApplication(this IServiceCollection services)
        {
            

            services.AddScoped<IInventoryService,InventoryService>();

        }
    }
}
