using Microsoft.EntityFrameworkCore;
using Sales.Application.Interfaces;
using Sales.Application.Services;
using Sales.Domain.Repositories;
using Sales.Infrastructure;
using Sales.Infrastructure.Persistence;
using Sales.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Register DbContext (using InMemory for simplicity; replace with SQL Server if needed)
builder.Services.AddDbContext<SalesDbContext>(options =>
    options.UseInMemoryDatabase("SalesDb"));

// Register repositories
builder.Services.AddScoped<ISaleRepository, SaleRepository>();

// Register application services
builder.Services.AddScoped<ISaleService, SaleService>();


// Learn more about configuring Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
