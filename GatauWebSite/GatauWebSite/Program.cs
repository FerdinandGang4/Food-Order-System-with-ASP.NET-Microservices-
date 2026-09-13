using GatauWebSite.Model;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<Database>(options => options.UseInMemoryDatabase("GatauDatabase"));

var app = builder.Build();

// GET: Get all products
app.MapGet("/products", async (Database database) =>
{
    return await database.Products.ToListAsync();
});

// GET: Get one product by ID
app.MapGet("/products/{id:int}", async (int id, Database database) =>
{
    var product = await database.Products.FindAsync(id);

    return product is not null
        ? Results.Ok(product)
        : Results.NotFound($"Product with ID {id} was not found.");
});

// POST: Create a product
app.MapPost("/products", async (Product product, Database database) =>
{
    database.Products.Add(product);
    await database.SaveChangesAsync();

    return Results.Created($"/products/{product.Id}", product);
});

// PUT: Update an existing product
app.MapPut("/products/{id:guid}", async (
    Guid id,
    Product updatedProduct,
    Database database) =>
{
    var existingProduct = await database.Products.FindAsync(id);

    if (existingProduct is null)
    {
        return Results.NotFound(
            $"Product with ID {id} was not found.");
    }

    existingProduct.Name = updatedProduct.Name;
    existingProduct.Description = updatedProduct.Description;
    existingProduct.Price = updatedProduct.Price;
    existingProduct.ImageUrl = updatedProduct.ImageUrl;
    existingProduct.IsActive = updatedProduct.IsActive;
    existingProduct.UpdatedAt = DateTime.UtcNow;

    await database.SaveChangesAsync();

    return Results.Ok(existingProduct);
});

// DELETE: Delete a product
app.MapDelete("/products/{id:int}", async (int id, Database database) =>
{
    var product = await database.Products.FindAsync(id);

    if (product is null)
    {
        return Results.NotFound($"Product with ID {id} was not found.");
    }

    database.Products.Remove(product);
    await database.SaveChangesAsync();

    return Results.NoContent();
});


app.Run();
