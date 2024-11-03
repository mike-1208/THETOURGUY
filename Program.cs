using Microsoft.EntityFrameworkCore;
using Application.Services;
using Domain.Interfaces;
using Infrastructure.External.BigGuy;
using Infrastructure.External.SomeOtherGuy;
using Infrastructure.Persistance;
using Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register all IProductRepository implementations
builder.Services.AddScoped<IProductRepository, BigGuyProductRepository>();
builder.Services.AddScoped<IProductRepository, SomeOtherGuyProductRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();

// Configure DbContext with SQLite
builder.Services.AddDbContext<TourGuyDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register ProductRepositoryAggregator and inject all repositories
builder.Services.AddScoped<ProductRepositoryAggregator>(serviceProvider =>
{
    var repositories = serviceProvider.GetServices<IProductRepository>().ToList();
    return new ProductRepositoryAggregator(repositories);
});

// Register other services
builder.Services.AddScoped<ProductService>();
builder.Services.AddLogging();

var app = builder.Build();

// Apply database migrations and populate data if the database is empty
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<TourGuyDbContext>();
    
    // Ensure database is created and migrations are applied
    context.Database.Migrate();
    
    // Populate database with initial data if empty
    if (!context.Products.Any())
    {
        PopulateDbFromJson.Populate(context);
    }
}

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
