using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProductWebApi.Exceptions;
using ProductWebApi.Models;
using ProductWebApi.Services;
using System.Linq;
using System.Threading.Tasks;


namespace ProductWebApi.Tests
{
    [TestClass]
    public class ProductServiceTests
    {
        private DbContextOptions<ProductContext> GetDbSetOptions(string dbName)
        {
            return new DbContextOptionsBuilder<ProductContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;
        }


        [TestMethod]
        public async Task GetProductsReturnsEmptyList()
        {
            using var productContext = new ProductContext(GetDbSetOptions("test1"));
            var products = await new ProductService(productContext).GetProductsAsync();
            Assert.IsNotNull(products);
            Assert.AreEqual(products.Count(), 0);
        }

        [TestMethod]
        public async Task GetProductsReturnsOneProjectInList()
        {
            using var productContext = new ProductContext(GetDbSetOptions("test2"));
            var product = new Product { Id = "Test1", Description = "Test Product 1", Brand = "Brand A", Model = "Model A" };
            productContext.Add(product);
            productContext.SaveChanges();

            var products = await new ProductService(productContext).GetProductsAsync();
            Assert.AreEqual(products.Count(), 1);
            Assert.AreEqual(products.First().Id, product.Id);
        }

        [TestMethod]
        public async Task CreateProductAddsProduct()
        {
            using var productContext = new ProductContext(GetDbSetOptions("test3"));
            var product = new Product { Id = "Test New", Description = "New Test Product", Brand = "Brand A", Model = "Model A" };

            await new ProductService(productContext).CreateProductAsync(product);
            Assert.AreEqual(productContext.Products.Count(), 1);
            Assert.AreEqual(productContext.Products.First().Id, product.Id);
        }

        [TestMethod]
        public async Task UpdateNonExistentProductFails()
        {
            using var productContext = new ProductContext(GetDbSetOptions("test4"));
            var product = new Product { Id = "Test Updated", Description = "Updated Test Product", Brand = "Brand Z", Model = "Model Y" };

            await Assert.ThrowsExceptionAsync<DataNotFoundException>(() => new ProductService(productContext).UpdateProductAsync(product));
        }

        [TestMethod]
        public async Task UpdateExistingProductSucceeds()
        {
            using var productContext = new ProductContext(GetDbSetOptions("test5"));
            var product = new Product { Id = "Test1", Description = "Test Product 1", Brand = "Brand A", Model = "Model A" };
            productContext.Add(product);
            productContext.SaveChanges();

            foreach (var entity in productContext.ChangeTracker.Entries())
            {
                entity.State = EntityState.Detached;
            }

            var updatedProduct = new Product { Id = "Test1", Description = "Updated Test Product", Brand = "Brand Z", Model = "Model Y" };

            await new ProductService(productContext).UpdateProductAsync(updatedProduct);
            Assert.AreEqual(productContext.Products.First().Id, updatedProduct.Id);
            Assert.AreEqual(productContext.Products.First().Description, updatedProduct.Description);
            Assert.AreEqual(productContext.Products.First().Brand, updatedProduct.Brand);
            Assert.AreEqual(productContext.Products.First().Model, updatedProduct.Model);
        }

        [TestMethod]
        public async Task DeleteNonExistentProductFails()
        {
            using var productContext = new ProductContext(GetDbSetOptions("test6"));
            await Assert.ThrowsExceptionAsync<DataNotFoundException>(() => new ProductService(productContext).DeleteProductAsync("ZZZ1"));
        }

        [TestMethod]
        public async Task DeleteProductSucceeds()
        {
            using var productContext = new ProductContext(GetDbSetOptions("test7"));
            var product = new Product { Id = "Test1", Description = "Test Product 1", Brand = "Brand A", Model = "Model A" };
            productContext.Add(product);
            productContext.SaveChanges();

            foreach (var entity in productContext.ChangeTracker.Entries())
            {
                entity.State = EntityState.Detached;
            }
            await new ProductService(productContext).DeleteProductAsync("Test1");
            Assert.AreEqual(productContext.Products.Count(), 0);
        }
    }
}
