using Microsoft.EntityFrameworkCore;
using Application.Services;
using Domain.Interfaces;
using Infrastructure.External.BigGuy;
using Infrastructure.External.SomeOtherGuy;
using Infrastructure.Persistance;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IProductRepository, BigGuyProductRepository>();
builder.Services.AddScoped<IProductRepository, SomeOtherGuyProductRepository>();

builder.Services.AddDbContext<TourGuyDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ProductRepositoryAggregator>();
builder.Services.AddScoped<ProductService>();


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
