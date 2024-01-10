using Microsoft.AspNetCore.Mvc;
using MinimalApiDemo.Data;
using MinimalApiDemo.Models;

namespace MinimalApiDemo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthorization();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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

            var summaries = new[]
            {
                "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
            };

            var new_products = new Producto[]
            {

            };

            app.MapGet("/weatherforecast", (HttpContext httpContext) =>
            {
                var forecast = Enumerable.Range(1, 5).Select(index =>
                    new WeatherForecast
                    {
                        Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                        TemperatureC = Random.Shared.Next(-20, 55),
                        Summary = summaries[Random.Shared.Next(summaries.Length)]
                    })
                    .ToArray();
                return forecast;
            })
            .WithName("GetWeatherForecast")
            .WithOpenApi();

            //Minimal API demo for products
            app.MapGet("/api/producto", (HttpContext httpContext, ILogger<Program> _logger) => {
                _logger.Log(LogLevel.Information, "Getting all products...");
                return Results.Ok(ProductoStore.product_list);
            })
            .WithName("GetProductos")
            .WithOpenApi()
            .Produces<IEnumerable<Producto>>(200);

            app.MapGet("/api/producto/{id:int}", (HttpContext httpContext, ILogger<Program> _logger, int id) => {
                _logger.Log(LogLevel.Information, "Getting one product...");
                return Results.Ok(ProductoStore.product_list.FirstOrDefault( product =>  product.Id == id));
            })
            .WithName("GetProducto")
            .WithOpenApi().Produces<Producto>(200);

            app.MapPost("/api/producto", (HttpContext httpContext, ILogger<Program> _logger, [FromBody] Producto producto) => {
                if (producto.Id != 0 || string.IsNullOrEmpty(producto.Name))
                {
                    return Results.BadRequest($"This product: {producto.ProductoInfo(producto)}\n\tis invalid...");
                }
                if (ProductoStore.product_list.FirstOrDefault(prod => prod.Name.ToLower() == producto.Name.ToLower()) != null)
                {
                    return Results.BadRequest("Product name already exists...");
                }
                producto.Id = ProductoStore.product_list.OrderByDescending(prod => prod.Id).FirstOrDefault().Id + 1;
                ProductoStore.product_list.Add(producto);
                _logger.Log(LogLevel.Information, "Product created successfully");
                return Results.CreatedAtRoute("GetProducto", new { id = producto.Id }, producto);
                //return Results.Created($"/api/producto/{producto.Id}", producto);
                //return Results.Ok(producto);
            })
            .WithName("PostProducto")
            .WithOpenApi()
            .Accepts<Producto>("application/json")
            .Produces<Producto>(200).Produces(400);

            app.MapPut("/api/producto/{id:int}", (HttpContext httpContext, int id) => {
                return Results.Ok(ProductoStore.product_list.FirstOrDefault(product => product.Id == id));
            })
            .WithName("PutProducto")
            .WithOpenApi();

            app.MapDelete("/api/producto/{id:int}", (HttpContext httpContext, int id) => {
                return Results.Ok(ProductoStore.product_list.FirstOrDefault(product => product.Id == id));
            })
            .WithName("DeleteProducto")
            .WithOpenApi();

            app.MapPatch("/api/producto/{id:int}", (HttpContext httpContext, int id) => {
                return Results.Ok(ProductoStore.product_list.FirstOrDefault(product => product.Id == id));
            })
            .WithName("PatchProducto")
            .WithOpenApi();

            app.Run();
        }
    }
}
