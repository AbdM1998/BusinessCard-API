using BusinessCardAPI.CultureProviders;
using BusinessCardAPI.Data;
using BusinessCardAPI.Interfaces.Repositories;
using BusinessCardAPI.Interfaces.Services;
using BusinessCardAPI.Repositories;
using BusinessCardAPI.Services;
using BusinessCardAPI.Services.Interfaces;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Resources;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


// For SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions => sqlOptions.EnableRetryOnFailure()
    ));


// CORS Configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});


builder.Services.AddScoped<IBusinessCardRepository, BusinessCardRepository>();
builder.Services.AddScoped<IBusinessCardService, BusinessCardService>();
builder.Services.AddScoped<IFileImportService, FileImportService>();
builder.Services.AddScoped<IExportService, ExportService>();



// Localization Configuration
builder.Services.AddLocalization(options => options.ResourcesPath = "");

var supportedCultures = new List<CultureInfo>()
                {
                    new CultureInfo("ar-JO"),
                    new CultureInfo("en-US")
                };


builder.Services.Configure<RequestLocalizationOptions>(options =>
{
  
    options.DefaultRequestCulture = new RequestCulture(culture: "en-US", uiCulture: "en-US");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
    options.RequestCultureProviders = new[] { new UserProfileRequestCultureProvider() };
});

var app = builder.Build();

app.UseRequestLocalization(new RequestLocalizationOptions()
{
    DefaultRequestCulture = new RequestCulture(culture: "en-US", uiCulture: "en-US"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures,
    RequestCultureProviders = new[] { new UserProfileRequestCultureProvider() }
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors("CorsPolicy");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseMiddleware<MigrationMiddleware>();


app.Run();
