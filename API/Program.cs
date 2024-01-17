using API;
using API.Extensions;
using API.Interfaces;
using API.Middleware;
using API.Services;
using Microsoft.EntityFrameworkCore;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddDbContext<DataContext>(opt => { 
    opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
}); 

builder.Services.AddCors(); 
builder.Services.AddScoped<ITokenService,TokenService>();
builder.Services.AddIdentityServices(builder.Configuration);
var app = builder.Build();


// Configure the HTTP request pipeline.
app.UseMiddleware<ExceptionMiddleware>();
app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200"));
app.UseAuthentication();// do you have a valid token
app.UseAuthorization();// if you have a valid token what you allowed to do 

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
