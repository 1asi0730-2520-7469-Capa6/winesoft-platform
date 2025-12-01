using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using QuestPDF.Infrastructure;
using WinesoftPlatform.API.Analytics.Domain.Services;
using WinesoftPlatform.API.Shared.Domain.Repositories;
using WinesoftPlatform.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using WinesoftPlatform.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using WinesoftPlatform.API.Shared.Infrastructure.Interfaces.ASAP.Configuration;
using WinesoftPlatform.API.Inventory.Domain.Repositories;
using WinesoftPlatform.API.Inventory.Domain.Services;
using WinesoftPlatform.API.Inventory.Application.Internal.CommandServices;
using WinesoftPlatform.API.Inventory.Application.Internal.QueryServices;
using WinesoftPlatform.API.Inventory.Infrastructure.Persistence.Repositories;
using WinesoftPlatform.API.IAM.Infrastructure.Extensions;
using WinesoftPlatform.API.Dashboard.Infrastructure.Interfaces.ASP.Configuration.Extensions;
using WinesoftPlatform.API.Purchase.Domain.Repositories;
using WinesoftPlatform.API.Purchase.Infrastructure.Persistence.EFC.Repositories;
using WinesoftPlatform.API.Profiles.Infrastructure.Interfaces.ASP.Configuration.Extensions;
using WinesoftPlatform.API.Analytics.Infrastructure.Interfaces.ASP.Configuration.Extensions;
using WinesoftPlatform.API.Analytics.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

QuestPDF.Settings.License = LicenseType.Community;

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllPolicy",
        policy => policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

builder.Services.AddControllers(options =>
    options.Conventions.Add(new KebabCaseRouteNamingConvention()));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => options.EnableAnnotations());
builder.Services.AddOpenApi();

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDbContext<AppDbContext>(options =>
    {
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        if (connectionString is null)
            throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        
        options.UseMySQL(connectionString)
            .LogTo(Console.WriteLine, LogLevel.Information)
            .EnableSensitiveDataLogging()
            .EnableDetailedErrors();
    });
}
else
{
    builder.Services.AddDbContext<AppDbContext>(options =>
    {
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        if (connectionString is null)
            throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        
        options.UseMySQL(connectionString);
    });
}

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Inventory
builder.Services.AddScoped<ISupplyRepository, SupplyRepository>();
builder.Services.AddScoped<ISupplyCommandService, SupplyCommandService>();
builder.Services.AddScoped<ISupplyQueryService, SupplyQueryService>();

// Purchase
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

builder.AddProfilesContextServices();

// Register IAM module: HTTP client to external login service, repository and application services.
// Expected configuration in appsettings: "IAM:AuthBaseUrl" (base URL) and "IAM:SigninPath" (signin path).
builder.Services.AddIAM(builder.Configuration);
builder.AddAnalyticsContextServices();

builder.Services.AddLocalization();
builder.Services.AddScoped<IAnalyticsReportBuilder, QuestPdfAnalyticsReportBuilder>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.EnsureCreated();
}

app.UseHttpsRedirection();
app.UseCors("AllowAllPolicy");
app.UseAuthorization();
app.MapControllers();
app.Run();