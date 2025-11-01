using Microsoft.EntityFrameworkCore;
using Portal.Usuario.Application.Handlers;
using Portal.Usuario.Application.Queries;
using Portal.Usuario.Core.Entities;
using Portal.Usuario.Core.Interfaces;
using Portal.Usuario.Infrastructure.DatabaseServices.Context;
using Portal.Usuario.Infrastructure.DatabaseServices.Repository;

var builder = WebApplication.CreateBuilder(args);

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

//Repository DI
builder.Services.AddScoped<IApplicationDbRepository<User>, ApplicationDbRepository<User>>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
