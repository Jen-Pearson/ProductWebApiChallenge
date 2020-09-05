using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using ProductWebApi.Models;

namespace ProductWebApi.Tests
{
    [TestClass]
   public class ProductsApiIntegrationTest
    {
        private HttpClient _client;
        public ProductsApiIntegrationTest()
        {
            var server = new TestServer(new WebHostBuilder()
                    .UseEnvironment("Development")
                    .UseStartup<Startup>());

            _client = server.CreateClient();
        }

        [TestMethod]
        public async Task ProductsGetAllTestAsync()
        {
            var request = new HttpRequestMessage(new HttpMethod("GET"), "/products");
            var response = await _client.SendAsync(request);

            response.EnsureSuccessStatusCode();
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var stringResponse = response.Content.ReadAsStringAsync().Result;
            Assert.IsInstanceOfType(JsonConvert.DeserializeObject<List<Product>>(stringResponse), typeof(List<Product>));
        }

        [TestMethod]
        public async Task ProductPostGetAllReturnsOneTestAsync()
        {
            var newProduct = new Product { Id = "A", Description = "Test Product A", Model = "Model A", Brand = "Brand A" };

            var productRequest = CreateRequestMessage("POST", "/products", newProduct);

            var createResponse = await _client.SendAsync(productRequest);

            createResponse.EnsureSuccessStatusCode();
            Assert.AreEqual(HttpStatusCode.Created, createResponse.StatusCode);

            var request = new HttpRequestMessage(new HttpMethod("GET"), $"/products");
            var response = await _client.SendAsync(request);

            response.EnsureSuccessStatusCode();
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var stringResponse = response.Content.ReadAsStringAsync().Result;
            var productList = JsonConvert.DeserializeObject<List<Product>>(stringResponse);
            Assert.AreEqual(productList.Where(p=>p.Id == newProduct.Id).Any(), true);

        }

        [TestMethod]
        public async Task ProductPostGetByIdTestAsync()
        {
            var newProduct = new Product { Id = "B", Description = "Test Product B", Model = "Model B", Brand = "Brand B" };
            var postRequest = CreateRequestMessage("POST", "/products", newProduct);
            var createResponse = await _client.SendAsync(postRequest);

            createResponse.EnsureSuccessStatusCode();
            Assert.AreEqual(HttpStatusCode.Created, createResponse.StatusCode);

            var request = new HttpRequestMessage(new HttpMethod("GET"), $"/products/{newProduct.Id}");
            var response = await _client.SendAsync(request);

            response.EnsureSuccessStatusCode();
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var stringResponse = response.Content.ReadAsStringAsync().Result;
            var product = JsonConvert.DeserializeObject<Product>(stringResponse);
            Assert.AreEqual(product.Id, newProduct.Id);
            Assert.AreEqual(product.Description, newProduct.Description);
            Assert.AreEqual(product.Model, newProduct.Model);
            Assert.AreEqual(product.Brand, newProduct.Brand);
        }

        [TestMethod]
        public async Task NewProductPutFailsTestAsync()
        {
            var newProduct = new Product { Id = "D", Description = "Test Product D", Model = "Model D", Brand = "Brand D" };
            var productRequest = CreateRequestMessage("PUT", $"/products/{newProduct.Id}", newProduct);
            
            var createResponse = await _client.SendAsync(productRequest);
            
            Assert.AreEqual(HttpStatusCode.NotFound, createResponse.StatusCode);
        }

        [TestMethod]
        public async Task ExistingProductPutSucceedsTestAsync()
        {
            var newProduct = new Product { Id = "E", Description = "Test Product E", Model = "Model E", Brand = "Brand E" };
            var productRequest = CreateRequestMessage("POST", $"/products", newProduct);
            var createResponse = await _client.SendAsync(productRequest);
            createResponse.EnsureSuccessStatusCode();

            newProduct.Description = "Updated Description";
            newProduct.Model = "Updated Model";
            newProduct.Brand = "Updated Brand";

            var updateRequest = CreateRequestMessage("PUT", $"/products/{newProduct.Id}", newProduct);

            var updatedResponse = await _client.SendAsync(updateRequest);
            updatedResponse.EnsureSuccessStatusCode();

            var request = new HttpRequestMessage(new HttpMethod("GET"), $"/products/{newProduct.Id}");
            var response = await _client.SendAsync(request);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            
            var stringResponse = response.Content.ReadAsStringAsync().Result;
            var product = JsonConvert.DeserializeObject<Product>(stringResponse);
            Assert.AreEqual(product.Id, newProduct.Id);
            Assert.AreEqual(product.Description, "Updated Description");
            Assert.AreEqual(product.Model, "Updated Model");
            Assert.AreEqual(product.Brand, "Updated Brand");

        }

        [TestMethod]
        public async Task NoProductDeleteFailsTestAsync()
        {
            var deleteRequest = new HttpRequestMessage(new HttpMethod("DELETE"), $"/products/UNKNOWN");
            var deleteResponse = await _client.SendAsync(deleteRequest);

            Assert.AreEqual(HttpStatusCode.NotFound, deleteResponse.StatusCode);
        }

        private static HttpRequestMessage CreateRequestMessage(string verb, string url, Product product)
        {
            return new HttpRequestMessage(new HttpMethod(verb), url)
            {
                Content = new StringContent(JsonConvert.SerializeObject(product),
                            System.Text.Encoding.UTF8,
                            "application/json")
            };
        }
    }
}
