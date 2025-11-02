using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Portal.Usuario.Application.Handlers;
using Portal.Usuario.Application.Queries;
using Portal.Usuario.Core.Entities;
using Portal.Usuario.Core.Interfaces;
using Portal.Usuario.Infrastructure.DatabaseServices.Context;
using Portal.Usuario.Infrastructure.DatabaseServices.Repository;
using Portal.Usuario.Infrastructure.DatabaseServices.Seeders;
using System;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp",
        policy =>
        {
            policy.WithOrigins("http://localhost:4200", "http://localhost:4000")
                  .AllowCredentials()
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});


//JWT Config
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]!);

builder.Services.AddAuthentication(opt =>
{
    opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(opt =>
{
    opt.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

builder.Services.AddAuthorization();

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(opt => opt.UseInMemoryDatabase("UserDataBase"));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped(typeof(IApplicationDbRepository<>), typeof(ApplicationDbRepository<>));
builder.Services.AddScoped<DbContext, ApplicationDbContext>();
//Queries DI

//Mediatr DI
builder.Services.AddMediatR(opt => opt.RegisterServicesFromAssembly(typeof(GetUsersByQuery).Assembly));
builder.Services.AddMediatR(opt => opt.RegisterServicesFromAssembly(typeof(CreateUserHandler).Assembly));
builder.Services.AddMediatR(opt => opt.RegisterServicesFromAssembly(typeof(CreateTokenHandler).Assembly));

//Repository DI
builder.Services.AddScoped<IApplicationDbRepository<User>, ApplicationDbRepository<User>>();


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    UserDbSeeder.Seed(dbContext);
}

app.UseCors("AllowAngularApp");
// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Use(async (context, next) =>
{
    if (context.Request.Path == "/index.html" || context.Request.Path == "/")
        context.Response.Redirect("/swagger/index.html");
    else
        await next();
});

app.Run();
