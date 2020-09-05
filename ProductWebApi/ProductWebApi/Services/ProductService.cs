using ProductWebApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using ProductWebApi.Exceptions;

namespace ProductWebApi.Services
{
    public class ProductService : IProductService
    {
        private readonly ProductContext _context;

        public ProductService(ProductContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsAsync(string model, string description, string brand)
        {
            var filteredProducts = from p in _context.Products
                                   where (string.IsNullOrEmpty(model) || p.Model.Contains(model)) &&
                                   (string.IsNullOrEmpty(description) || p.Description.Contains(description)) &&
                                   (string.IsNullOrEmpty(brand) || p.Brand.Contains(brand))
                                   select p;

            return await filteredProducts.ToListAsync();
        }

        public async Task<Product> GetProductAsync(string id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task UpdateProductAsync(Product product)
        {
            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(product.Id))
                {
                    throw new DataNotFoundException();
                }
                else
                {
                    throw;
                }
            }

        }

        public async Task<Product> CreateProductAsync(Product product)
        {
            _context.Products.Add(product);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ProductExists(product.Id))
                {
                    throw new DataConflictException();
                }
                else
                {
                    throw;
                }
            }

            return product;
        }

        public async Task<Product> DeleteProductAsync(string id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                throw new DataNotFoundException();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return product;
        }

        private bool ProductExists(string id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
