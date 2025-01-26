using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("WeatherLimiter", limiterOptions =>
    {
        limiterOptions.PermitLimit = 5;
        limiterOptions.Window = TimeSpan.FromHours(1);
        limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        limiterOptions.QueueLimit = 0;
    });
});

builder.Services.AddControllers();


// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

// Serve static files from the React build folder (for production)
app.UseDefaultFiles();
//app.UseStaticFiles(); 

// Set up the development proxy for React (if in development mode)
if (app.Environment.IsDevelopment())
{
    app.UseSpa(spa =>
    {
        spa.Options.SourcePath = "Frontend"; // Path to the React app folder

        // Proxy to the React development server
        spa.UseProxyToSpaDevelopmentServer("http://localhost:5173/");
    });
}

app.Run();