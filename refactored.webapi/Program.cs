using log4net.Config;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using reafactored.services;
using reafactored.services.InterFace;
using refactored.dal;
using refactored.services;
using refactored.services.InterFace;
using System.Configuration;
 
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

var configuration = new ConfigurationBuilder()
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", optional: true)
.Build();

string rootDirectory = AppDomain.CurrentDomain.BaseDirectory + @"App_Data";
 var connectionString = configuration.GetConnectionString("DefaultConnection").Replace("{SpecialFolder}", rootDirectory);

XmlConfigurator.ConfigureAndWatch(new FileInfo("log4net.config"));

builder.Services.AddDbContext<RefactoredDBContext>(options =>
{
    options.UseSqlServer(connectionString);
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<IMyConnection, MyConnection>();
builder.Services.AddTransient<IProductInterface, ProductsService>();

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
