using FlowtrixAi.Client;
using FlowtrixAi.Client.Services;
using FlowtrixAi.Client.Handlers;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// تسجيل الوسيط الخاص بالمصادقة
builder.Services.AddScoped<AuthHandler>();

// تسجيل HttpClient مع ربطه بالوسيط المخصص
builder.Services.AddHttpClient("ServerAPI", client => 
{
    client.BaseAddress = new Uri("https://localhost:44350");
})
.AddHttpMessageHandler<AuthHandler>();

// جعل HttpClient الافتراضي يستخدم الإعدادات أعلاه
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("ServerAPI"));

builder.Services.AddScoped<ThemeService>();
builder.Services.AddScoped<ProductionOrderClientService>();
builder.Services.AddScoped<InventoryClientService>();
builder.Services.AddScoped<ProductClientService>();

await builder.Build().RunAsync();
