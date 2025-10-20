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

app.UseHttpsRedirection();

app.UseRouting();
app.UseCors("AllowAll");

app.UseAuthorization();
app.MapControllers();

app.UseStaticFiles(); // (optional) static files can be before/after, not critical for CORS
app.UseStaticFiles(new StaticFileOptions { /* qrcodes mapping */ });

app.Run();
