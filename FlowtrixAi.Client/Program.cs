using FlowtrixAi.Client;
using FlowtrixAi.Client.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:44350") });
builder.Services.AddScoped<ThemeService>();
builder.Services.AddScoped<ProductionOrderClientService>();
builder.Services.AddScoped<InventoryClientService>();
builder.Services.AddScoped<ProductClientService>();

await builder.Build().RunAsync();
