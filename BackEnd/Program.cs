var builder = WebApplication.CreateBuilder(args);

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