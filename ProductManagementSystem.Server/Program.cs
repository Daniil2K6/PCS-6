using Microsoft.EntityFrameworkCore;
using ProductManagementSystem.Server.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorClient", policy =>
    {
        policy.WithOrigins("https://localhost:7279", "http://localhost:5279")
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Add Entity Framework Core
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
        ?? "Server=(localdb)\\mssqllocaldb;Database=ProductManagementSystemDb;Trusted_Connection=true;";
    options.UseSqlServer(connectionString);
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    
    // Apply migrations automatically
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        dbContext.Database.EnsureCreated();
    }
}

app.UseCors("AllowBlazorClient");
app.UseHttpsRedirection();
app.MapControllers();

app.Run();
