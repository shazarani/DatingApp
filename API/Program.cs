using System.Text;
using API;
using API.Extensions;
using API.Interfaces;
using API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;


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

app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200"));
app.UseAuthentication();// do you have a valid token
app.UseAuthorization();// if you have a valid token what you allowed to do 

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
