using Infra;
using Core;
using FluentValidation.AspNetCore;
using Core.Middleware;
using Microsoft.Extensions.Hosting;
using Infra.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Api2;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDatabaseContext(config.GetConnectionString("DefaultConnection")!);
builder.Services.AddMvc();
builder.Services.AddCors();
builder.Services.AddAuth();
builder.Services.AddSwaggerSetup();
builder.Services.RegisterCoreDependencies();

builder.Services.AddFluentValidationAutoValidation();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddleware>();
app.UseRouting();

app.UseCors(options =>
{
    options.WithOrigins(
        "http://localhost:4200")
        .AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod();
});

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
