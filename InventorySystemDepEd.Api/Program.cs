using InventorySystemDepEd.Api.Services;
using InventorySystemDepEd.Api.Data;
using InventorySystemDepEd.Api.Services.Excel;
using InventorySystemDepEd.Shared.Excel;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 🔥 API SETTINGS
var host = builder.Configuration["ApiSettings:Host"] ?? "localhost";
var port = builder.Configuration["ApiSettings:Port"] ?? "5170";

// 🔥 Kestrel binding
builder.WebHost.UseUrls($"http://{host}:{port}");

// ==========================
// 🔥 DATABASE (POSTGRES)
// ==========================
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("Default")
    )
);

// ==========================
// 🔥 CONTROLLERS (IMPORTANT)
// ==========================
builder.Services.AddControllers();

// ==========================
// 🔥 EXCEL SERVICES
// ==========================
builder.Services.AddScoped<IExcelTemplateService, ExcelTemplateService>();
builder.Services.AddScoped<IExcelImportService, ExcelImportService>();
builder.Services.AddScoped<PositionProvider>();
builder.Services.AddScoped<OfficeProvider>();
builder.Services.AddScoped<IExcelTemplateRegistry, ExcelTemplateRegistry>();

// ==========================
// 🔥 SWAGGER
// ==========================
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ==========================
// 🔥 CORS (FOR BLAZOR CLIENT)
// ==========================
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var app = builder.Build();

// ==========================
// 🔥 MIDDLEWARE PIPELINE
// ==========================

// Swagger (always on dev/internal API)
app.UseSwagger();
app.UseSwaggerUI();

// CORS (must be before auth/controllers)
app.UseCors("AllowAll");

// HTTPS redirect (disabled - using HTTP only for development)
// app.UseHttpsRedirection();

// Authorization (you can add JWT later)
app.UseAuthorization();

// 🔥 MAP CONTROLLERS (CRITICAL)
app.MapControllers();

app.Run();