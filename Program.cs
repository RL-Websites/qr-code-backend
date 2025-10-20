using Microsoft.Extensions.FileProviders;
using System.IO;

var builder = WebApplication.CreateBuilder(args);

// ==========================
// Add services to the container
// ==========================
builder.Services.AddControllers();

// --------------------------
// Configure CORS
// --------------------------
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("https://qrcode.rldhaka.com") // <-- your frontend domain
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// --------------------------
// Add OpenAPI / Swagger if needed
// --------------------------
builder.Services.AddOpenApi();

var app = builder.Build();

// ==========================
// Middleware pipeline
// ==========================

// Enable CORS
app.UseCors("AllowFrontend");

// Serve static files from wwwroot
app.UseStaticFiles(); // default wwwroot

// Serve specifically /wwwroot/qrcodes under /qrcodes path
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "qrcodes")),
    RequestPath = "/qrcodes"
});

// Configure HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseOpenApi();
    app.UseSwaggerUi3();
}

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();
