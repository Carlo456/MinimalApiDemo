using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using MinimalApiDemo.Data;
using MinimalApiDemo.Models;
using MinimalApiDemo.Models.DTO;
using System.Net;

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
            builder.Services.AddAutoMapper(typeof(MappingConfig));
            builder.Services.AddValidatorsFromAssemblyContaining<Program>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            //Minimal API demo for products
            app.MapGet("/api/product", (HttpContext httpContext, ILogger<Program> _logger) => {
                _logger.Log(LogLevel.Information, "Getting all products...");
                APIResponse response = new();
                response.Result = ProductStore.product_list;
                response.Success = true;
                response.StatusCode = HttpStatusCode.OK;
                return Results.Ok(response);
            })
            .WithName("GetProducts")
            .WithOpenApi()
            .Produces<APIResponse>(200);

            app.MapGet("/api/product/{id:int}", (HttpContext httpContext, ILogger<Program> _logger, int id) => {
                _logger.Log(LogLevel.Information, "Getting one product...");
                APIResponse response = new();
                response.Result = ProductStore.product_list.FirstOrDefault(product => product.Id == id);
                response.Success = true;
                response.StatusCode = HttpStatusCode.OK;
                return Results.Ok(response);
                //return Results.Ok(ProductoStore.product_list.FirstOrDefault( product =>  product.Id == id));
            })
            .WithName("GetProducto")
            .WithOpenApi().Produces<APIResponse>(200);

            app.MapPost("/api/product", async (HttpContext httpContext, ILogger<Program> _logger, IMapper _mapper, IValidator<ProductCreateDTO> _validation, [FromBody] ProductCreateDTO product_c_dto) => {
                APIResponse response = new() { Success = false, StatusCode = HttpStatusCode.BadRequest };
                
                var validationResult = await _validation.ValidateAsync(product_c_dto);

                if (!validationResult.IsValid)
                {
                    response.ErrorMessages.Add(validationResult.Errors.FirstOrDefault().ToString());
                    return Results.BadRequest(response);
                }
                if (ProductStore.product_list.FirstOrDefault(prod => prod.Name.ToLower() == product_c_dto.Name.ToLower()) != null)
                {
                    response.ErrorMessages.Add("Coupon already exists...");
                    return Results.BadRequest(response);
                }

                Product product = _mapper.Map<Product>(product_c_dto);

                product.Id = ProductStore.product_list.OrderByDescending(prod => prod.Id).FirstOrDefault().Id + 1;
                ProductStore.product_list.Add(product);
                _logger.Log(LogLevel.Information, "Product created successfully");
                ProductDTO productDTO = _mapper.Map<ProductDTO>(product);

                response.Result = productDTO;
                response.Success = true;
                response.StatusCode = HttpStatusCode.Created;
                return Results.Ok(response);
                //return Results.CreatedAtRoute("GetProducto", new { id = product.Id }, productDTO);
                //return Results.Created($"/api/producto/{producto.Id}", producto);
                //return Results.Ok(producto);
            })
            .WithName("PostProduct")
            .WithOpenApi()
            .Accepts<ProductCreateDTO>("application/json")
            .Produces<APIResponse>(201).Produces(400);


            app.MapPut("/api/product/", async (HttpContext httpContext, ILogger<Program> _logger, IMapper _mapper, IValidator<ProductUpdateDTO> _validation, [FromBody] ProductUpdateDTO product_u_dto) => {
                APIResponse response = new() { Success = false, StatusCode = HttpStatusCode.BadRequest };

                var validationResult = await _validation.ValidateAsync(product_u_dto);

                if (!validationResult.IsValid)
                {
                    response.ErrorMessages.Add(validationResult.Errors.FirstOrDefault().ToString());
                    return Results.BadRequest(response);
                }
                if (ProductStore.product_list.FirstOrDefault(prod => prod.Name.ToLower() == product_u_dto.Name.ToLower()) != null)
                {
                    response.ErrorMessages.Add("Coupon already exists...");
                    return Results.BadRequest(response);
                }

                //get Product to update
                Product productFromStore = ProductStore.product_list.FirstOrDefault( p =>p.Id == product_u_dto.Id );
                productFromStore.Name = product_u_dto.Name;
                productFromStore.Description = product_u_dto.Description;
                productFromStore.PhotoUrl = product_u_dto.PhotoUrl;
                productFromStore.Price = product_u_dto.Price;
                productFromStore.Updated_at = DateTime.Now;

                _logger.Log(LogLevel.Information, "Product updated successfully");

                response.Result = _mapper.Map<ProductDTO>(productFromStore); ;
                response.Success = true;
                response.StatusCode = HttpStatusCode.OK;
                return Results.Ok(response);
            })
            .WithName("PutProduct")
            .WithOpenApi()
            .Accepts<ProductUpdateDTO>("application/json")
            .Produces<APIResponse>(200).Produces(400);

            app.MapDelete("/api/product/{id:int}", (HttpContext httpContext, ILogger<Program> _logger, int id) => {
                APIResponse response = new() { Success = false, StatusCode = HttpStatusCode.BadRequest };

                //get Product to delete
                Product productFromStore = ProductStore.product_list.FirstOrDefault(p => p.Id == id);
                if (productFromStore != null)
                {
                    ProductStore.product_list.Remove(productFromStore);
                    response.Success = true;
                    response.StatusCode = HttpStatusCode.NoContent;
                    _logger.Log(LogLevel.Information, "Product deleted successfully");
                    return Results.Ok(response);
                }
                else
                {
                    response.ErrorMessages.Add("Invalid Id");
                    return Results.BadRequest(response);
                } 
            })
            .WithName("DeleteProduct")
            .WithOpenApi();

            app.MapPatch("/api/product/{id:int}", (HttpContext httpContext, int id) => {
                return Results.Ok(ProductStore.product_list.FirstOrDefault(product => product.Id == id));
            })
            .WithName("PatchProduct")
            .WithOpenApi();

            app.Run();
        }
    }
}
