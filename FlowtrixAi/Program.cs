using FlowtrixAI.Api.Extensions;
using FlowtrixAI.Api.Middlewares;
using FlowtrixAI.Application.Extensions;
using FlowtrixAI.Infrastructure.Extensions;
using FlowtrixAI.Infrastructure.Seeders;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using FlowtrixAI.Domain.Entities;
using Microsoft.AspNetCore.StaticFiles;
using System.Collections.Concurrent;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    Args = args,
    ContentRootPath = AppContext.BaseDirectory
});
if (builder.Environment.IsDevelopment())
{
    builder.WebHost.UseStaticWebAssets();
}

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorClient",
        policy =>
        {
            policy.WithOrigins("http://localhost:4357", "https://localhost:44357")
                  .AllowAnyMethod()
                  .AllowAnyHeader()
                  .AllowCredentials();
        });
});



// Add application , infrastructure and presentation services
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.AddPresentation();

var key = builder.Configuration["Jwt:Key"];
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
            ClockSkew = TimeSpan.Zero,
            RoleClaimType = System.Security.Claims.ClaimTypes.Role 
        };
    });
builder.Services.AddAuthorization();



var app = builder.Build();
var frameworkAliasCache = new ConcurrentDictionary<string, string>(StringComparer.OrdinalIgnoreCase);

using (var scope = app.Services.CreateScope())
{
    // Automatic Migration & Database Creation
    var dbContext = scope.ServiceProvider.GetRequiredService<FlowtrixAI.Infrastructure.Context.AppDbContext>();
    await Microsoft.EntityFrameworkCore.RelationalDatabaseFacadeExtensions.MigrateAsync(dbContext.Database);

    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();
    await RoleSeeder.SeedRolesAsync(roleManager);

    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
    await UserSeeder.SeedAdminUserAsync(userManager);
}

app.UseCors("AllowBlazorClient");

app.Use(async (context, next) =>
{
    var requestPath = context.Request.Path.Value;
    if (string.IsNullOrWhiteSpace(requestPath))
    {
        await next();
        return;
    }

    static string? MatchFrameworkAsset(string webRootPath, string pattern, Func<string, bool>? extraFilter = null)
    {
        if (string.IsNullOrWhiteSpace(webRootPath))
        {
            return null;
        }

        var frameworkDir = Path.Combine(webRootPath, "_framework");
        if (!Directory.Exists(frameworkDir))
        {
            return null;
        }

        return Directory.EnumerateFiles(frameworkDir, pattern, SearchOption.TopDirectoryOnly)
                        .Select(Path.GetFileName)
                        .Where(file => !string.IsNullOrWhiteSpace(file))
                        .Where(file => extraFilter == null || extraFilter(file!))
                        .FirstOrDefault();
    }

    static string? ResolveFrameworkAlias(string requestPath, string webRootPath)
    {
        return requestPath.ToLowerInvariant() switch
        {
            "/_framework/blazor.webassembly.js" => MatchFrameworkAsset(webRootPath, "blazor.webassembly.*.js"),
            "/_framework/dotnet.js" => MatchFrameworkAsset(webRootPath, "dotnet.*.js", file => !file.StartsWith("dotnet.native.", StringComparison.OrdinalIgnoreCase) && !file.StartsWith("dotnet.runtime.", StringComparison.OrdinalIgnoreCase)),
            "/_framework/dotnet.native.js" => MatchFrameworkAsset(webRootPath, "dotnet.native.*.js"),
            "/_framework/dotnet.runtime.js" => MatchFrameworkAsset(webRootPath, "dotnet.runtime.*.js"),
            _ => null
        };
    }

    if (requestPath.StartsWith("/_framework/", StringComparison.OrdinalIgnoreCase))
    {
        var mappedPath = frameworkAliasCache.GetOrAdd(requestPath, key =>
        {
            var resolved = ResolveFrameworkAlias(key, app.Environment.WebRootPath);
            return string.IsNullOrWhiteSpace(resolved) ? string.Empty : $"/_framework/{resolved}";
        });

        if (!string.IsNullOrWhiteSpace(mappedPath))
        {
            context.Request.Path = mappedPath;
        }
    }

    await next();
});



if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseMiddleware<ErrorHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.MapStaticAssets();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
