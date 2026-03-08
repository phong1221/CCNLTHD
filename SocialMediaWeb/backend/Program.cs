using Backend.Controllers;
using Backend.Data;
using Backend.Middlewares;
using Backend.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
builder.Services.AddAuthorization();
// them service 
builder.Services.AddScoped<CategoryServicecs, CategoryServicecs>();
builder.Services.AddScoped<ReactService, ReactService>();
builder.Services.AddScoped<PostService,PostService>();
// set up db 
builder.Services.AddDbContext<BlogDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("Default"),
        ServerVersion.AutoDetect(
            builder.Configuration.GetConnectionString("Default"))));



var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// them middleaware de bat loi toan cuc
app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
