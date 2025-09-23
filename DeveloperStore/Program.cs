using Microsoft.EntityFrameworkCore;
using Sales.Application.Interfaces;
using Sales.Application.Services;
using Sales.Domain.Repositories;
using Sales.Infrastructure;
using Sales.Infrastructure.Persistence;
using Sales.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<SalesDbContext>(options => options.UseInMemoryDatabase("SalesDb"));

builder.Services.AddScoped<ISaleRepository, SaleRepository>();

builder.Services.AddScoped<ISaleService, SaleService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
