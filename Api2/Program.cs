using Api2.Infrastructure;
using Infra;
using Core;
using FluentValidation.AspNetCore;
using Api2.Services;
using Core.Middleware;

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
builder.Services.RegisterApiDependencies();

//builder.Services.AddValidatorsFromAssemblyContaining<CompanyValidator>();
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
