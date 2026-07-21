using Backend.Data;
using Backend.Interfaces.Authentication;
using Backend.Interfaces.Repository;
using Backend.Interfaces.Security;
using Backend.Interfaces.User;
using Backend.Repositories;
using Backend.Security;
using Backend.Services.Authentication;
using Backend.Services.Repository;
using Backend.Services.Security;
using Backend.Services.User;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IAuthenticationRepository, AuthenticationRepository>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
builder.Services.AddScoped<IPasswordEncoder, PasswordEncoder>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<ICookieService, CookieService>();

builder.Services.AddMemoryCache();
builder.Services.AddScoped<IRepoService, RepoService>();
builder.Services.AddScoped<IRepoRepository, RepoRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
// Add services to the container.
builder.Services.AddDbContext<DevHubDbContext>(options => { options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")); });
    
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("http://localhost:4200")
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();

    });
});
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]!))
    };
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var authHeader = context.Request.Headers["Authorization"].ToString();
            if (authHeader.StartsWith("Bearer "))
                context.Token = authHeader["Bearer ".Length..];
            else
                context.Token = context.Request.Cookies["accessToken"];
            return Task.CompletedTask;
        }
    };
   
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Use(async (context, next) =>
{
    context.Response.Headers.Append("X-Frame-Options", "DENY");
    context.Response.Headers.Append("Content-Security-Policy", "frame-ancestors 'none'");
    await next();
});
app.UseCors();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
