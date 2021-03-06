﻿using ProductWebApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductWebApi.Services
{
    public interface IProductService
    {
        Task<Product> CreateProductAsync(Product product);
        Task<Product> DeleteProductAsync(string id);
        Task<Product> GetProductAsync(string id);
        Task<IEnumerable<Product>> GetProductsAsync(string model, string description, string brand);
        Task UpdateProductAsync(Product product);
    }
}