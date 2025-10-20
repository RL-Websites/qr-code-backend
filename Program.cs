var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontends", policy =>
    {
        policy.WithOrigins(
            "https://qrcode.rldhaka.com", // no trailing slash
            "http://localhost:3000",      // dev (http)
            "https://localhost:3000"      // dev (https)
        )
        .AllowAnyHeader()
        .AllowAnyMethod();
        // .AllowCredentials(); // only if you actually send cookies/Authorization header
    });

    // Optional: a permissive DEV policy you can toggle via env
    //options.AddPolicy("AllowAllDev", policy =>
    //    policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

var app = builder.Build();

app.UseHttpsRedirection();

app.UseRouting();

// Pick ONE policy:
app.UseCors("Frontends");       // use this in prod
// if (app.Environment.IsDevelopment()) app.UseCors("AllowAllDev");

app.UseAuthorization();

app.MapControllers();
app.UseStaticFiles();

app.Run();
