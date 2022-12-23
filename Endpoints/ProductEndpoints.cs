using AutoMapper;
using DotNet_MinimalAPI_Example.Models.DTO;
using DotNet_MinimalAPI_Example.Models;
using DotNet_MinimalAPI_Example.Repository.IRepository;
using FluentValidation;
using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace DotNet_MinimalAPI_Example.Endpoints
{
    public static class ProductEndpoints
    {
        public static void ConfigureProductEndpoints(this WebApplication app)
        {
            // LIST
            app.MapGet("/api/products", ListProducts)
                .WithName("GetProducts").Produces<APIResponse>(200);

            // GET
            app.MapGet("/api/product/{id:int}", GetProduct)
                .WithName("GetProduct").Produces<APIResponse>(200);

            // POST
            app.MapPost("/api/product", CreateProduct)
                .WithName("CreateProduct").Accepts<ProductCreateDTO>("application/json").Produces<APIResponse>(201).Produces(400);

            // UPDATE
            app.MapPut("/api/product", UpdateProduct)
                .WithName("UpdateProduct").Accepts<ProductUpdateDTO>("application/json").Produces<APIResponse>(200).Produces(400);

            // DELETE
            app.MapDelete("/api/product/{id:int}", DeleteProduct);
        }

        private async static Task<IResult> ListProducts(
            ILogger<Program> _logger,
            IProductRepository _productRepo
        )
        {
            APIResponse response = new();
            _logger.LogInformation("Started Fetch for PRODUCT LIST:");
            response.Result = await _productRepo.GetAllAsync();
            response.IsSuccess = true;
            response.StatusCode = HttpStatusCode.OK;
            return Results.Ok(response);
        }



        private async static Task<IResult> GetProduct(
            int id, 
            ILogger<Program> _logger,
            IProductRepository _productRepo
        )
        {
            APIResponse response = new();
            _logger.LogInformation("Started Fetch for PRODUCT ID: " + id);
            response.Result = await _productRepo.GetAsync(id);
            response.IsSuccess = true;
            response.StatusCode = HttpStatusCode.OK;
            return Results.Ok(response);
        }


        private async static Task<IResult> CreateProduct(
            IMapper _mapper,
            IProductRepository _productRepo,
            ILogger<Program> _logger,
            IValidator<ProductCreateDTO> _validation,
            [FromBody] ProductCreateDTO createDTO
        )
        {
            // POST REQUEST HERE
            APIResponse response = new() { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest };
            _logger.LogInformation("Started Create Product");

            var validationResult = await _validation.ValidateAsync(createDTO);

            // Check if ID or name exists
            if (!validationResult.IsValid)
            {
                _logger.LogInformation("Failed to add Product, Error message: " + validationResult.Errors.FirstOrDefault().ToString());
                response.ErrorMessages.Add(validationResult.Errors.FirstOrDefault().ToString());
                return Results.BadRequest(response);
            }

            // Avoid the same product from having duplicates
            if (await _productRepo.GetAsync(createDTO.Name.ToLower()) != null)
            {
                _logger.LogInformation("Failed to add Product, Product Already Exists");
                response.ErrorMessages.Add("Product already exists");
                return Results.BadRequest(response);
            }

            Product product = _mapper.Map<Product>(createDTO);

            await _productRepo.CreateAsync(product); // Add new product to product list
            await _productRepo.SaveAsync();

            ProductDTO productDTO = _mapper.Map<ProductDTO>(product);


            _logger.LogInformation("Added New Product With ID + ", new { id = product.ProductId });
            response.Result = productDTO;
            response.IsSuccess = true;
            response.StatusCode = HttpStatusCode.Created;
            return Results.Ok(response);
        }
        
        
        private async static Task<IResult> UpdateProduct(
            IMapper _mapper,
            IProductRepository _productRepo,
            ILogger<Program> _logger,
            IValidator<ProductUpdateDTO> _validation,
            [FromBody] ProductUpdateDTO updateDTO
        )
        {
            // UPDATE REQUEST HERE
            APIResponse response = new() { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest };
            _logger.LogInformation("Started Update Product");

            var validationResult = await _validation.ValidateAsync(updateDTO);

            // Fluent Validation Checks
            if (!validationResult.IsValid)
            {
                _logger.LogInformation("Failed to update Product, Error message: " + validationResult.Errors.FirstOrDefault().ToString());
                response.ErrorMessages.Add(validationResult.Errors.FirstOrDefault().ToString());
                return Results.BadRequest(response);
            }

            await _productRepo.UpdateAsync(_mapper.Map<Product>(updateDTO));
            await _productRepo.SaveAsync();

            _logger.LogInformation("Updated Product With ID + ", updateDTO.ProductId);

            response.Result = _mapper.Map<ProductDTO>(await _productRepo.GetAsync(updateDTO.ProductId));
            response.IsSuccess = true;
            response.StatusCode = HttpStatusCode.OK;
            return Results.Ok(response);
        }


        private async static Task<IResult> DeleteProduct(
            ILogger<Program> _logger,
            IProductRepository _productRepo,
            int id
        )
        {
            // UPDATE REQUEST HERE
            APIResponse response = new() { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest };
            _logger.LogInformation("Started Delete Product");

            Product productFromStore = await _productRepo.GetAsync(id);

            if (productFromStore != null)
            {
                await _productRepo.DeleteAsync(productFromStore);
                await _productRepo.SaveAsync();
                response.IsSuccess = true;
                response.StatusCode = HttpStatusCode.NoContent;
                return Results.Ok(response);
            }
            else
            {
                _logger.LogInformation("Error creating product: Invalid Id");
                response.ErrorMessages.Add("Invalid Id");
                return Results.BadRequest(response);
            }
        }


    }
}
