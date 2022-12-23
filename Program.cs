using AutoMapper;
using DotNet_MinimalAPI_Example;
using DotNet_MinimalAPI_Example.Data;
using DotNet_MinimalAPI_Example.Endpoints;
using DotNet_MinimalAPI_Example.Models;
using DotNet_MinimalAPI_Example.Models.DTO;
using DotNet_MinimalAPI_Example.Repository;
using DotNet_MinimalAPI_Example.Repository.IRepository;
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Dependency Injections Here
// LOGGING
var logger = new LoggerConfiguration()
  .ReadFrom.Configuration(builder.Configuration)
  .Enrich.FromLogContext()
  .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

// AUTOMAPPER
builder.Services.AddAutoMapper(typeof(MappingConfig));

// FLUENTVALIDATION
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

// DATABASE
builder.Services.AddDbContext<ApplicationDBContext>(option =>
{
    option.UseNpgsql(builder.Configuration.GetConnectionString("DefaultSQLConnection"));
});

// REPOSITORIES
builder.Services.AddScoped<IProductRepository, ProductRepository>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configure Endpoints Here
app.ConfigureProductEndpoints();


app.UseHttpsRedirection();

app.Run();
