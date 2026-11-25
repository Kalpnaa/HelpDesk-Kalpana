using System.Runtime.CompilerServices;
using HelpDesk.Api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;


var builder = WebApplication.CreateBuilder(args);


//allow angular to connect
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

//connection to db
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));



// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

app.UseCors("AllowAngular");

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider
        .GetRequiredService<ApplicationDbContext>();

    var canConnect = await dbContext.Database.CanConnectAsync();

    Console.WriteLine($"Database connection successful: {canConnect}");
}


app.MapGet("/api/health",()=>
{
    return Results.Ok(
        new
        {
            message = "helpdesk api working",
            status = "okay"
            
        }
    );
});


app.Run();


