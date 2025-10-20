using Microsoft.Extensions.FileProviders;
using System.IO;

var builder = WebApplication.CreateBuilder(args);

// ==========================
// Add services
// ==========================
builder.Services.AddControllers();

// --------------------------
// Configure CORS
// --------------------------
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// ==========================
// Middleware pipeline
// ==========================

// Enable CORS
app.UseCors("AllowFrontend");

// Serve static files from wwwroot
app.UseStaticFiles();

// Serve /wwwroot/qrcodes under /qrcodes path
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "qrcodes")),
    RequestPath = "/qrcodes"
});

// Development exception page
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

// Routing & controllers
app.UseRouting();
app.UseAuthorization();
app.MapControllers();

app.Run();

// Routing & controllers
app.UseRouting();
app.UseAuthorization();
app.MapControllers();

app.Run();
