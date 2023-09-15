global using battleship_server.Models;
global using battleship_server.Services.ShipService;
global using battleship_server.DTOs.Ship;
global using AutoMapper;
global using Microsoft.EntityFrameworkCore;
global using battleship_server.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add the DbContext
builder.Services.AddDbContext<DataContext>(options 
    => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Service DI
builder.Services.AddScoped<IShipService, ShipService>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();

// Register the Auto Mapper
builder.Services.AddAutoMapper(typeof(Program).Assembly);

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
