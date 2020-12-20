using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitTestProject.Web.Controllers;
using UnitTestProject.Web.Helpers;
using UnitTestProject.Web.Models;
using UnitTestProject.Web.Repository;
using Xunit;

namespace UnitTestProject.Test
{
    public class ProductApiControllerTest
    {
        private readonly Mock<IRepository<Product>> _mock;
        private readonly ProductsApiController _productApiController;
        private List<Product> products;
        private readonly Helper _helper;
        public ProductApiControllerTest()
        {
            _helper = new Helper();
            _mock = new Mock<IRepository<Product>>();
            _productApiController = new ProductsApiController(_mock.Object);
            products = new List<Product>()
            {
                new Product()
                {
                    Color="TestColor",
                     Name ="TestName",
                      Price =4,
                       Stock =6,
                       Id =1
                },
                new Product()
                {
                    Color="TestColor1",
                     Name ="TestName1",
                      Price =5,
                       Stock =5,
                       Id =2
                },
                new Product()
                {
                    Color="TestColor3",
                     Name ="TestName4",
                      Price =33,
                       Stock =2,
                       Id =3
                }
            };
        }
        [Fact]
        public async void GetProduct_ActionExecutes_ReturnOkWithProduct()
        {
            _mock.Setup(x => x.GetAll()).ReturnsAsync(products);
            var result = await _productApiController.GetProducts();
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnProduct = Assert.IsAssignableFrom<IEnumerable<Product>>(okResult.Value);
            Assert.Equal<int>(3, returnProduct.Count());
        }
        [Theory]
        [InlineData(0)]
        public async void GetProduct_IdIsnull_ReturnNotFound(int productId)
        {
            Product product = null;
            _mock.Setup(x => x.GetById(productId)).ReturnsAsync(product);
            var result = await _productApiController.GetProduct(productId);
            Assert.IsType<OkObjectResult>(result);
        }
        [Theory]
        [InlineData(2)]
        [InlineData(3)]
        public async void GetProduct_Invalid_ReturnOkResult(int productId)
        {
            var product = products.First(x => x.Id == productId);
            _mock.Setup(x => x.GetById(productId)).ReturnsAsync(product);
            var result = await _productApiController.GetProduct(productId);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnProduct = Assert.IsType<Product>(okResult.Value);
            Assert.Equal<int>(product.Id, productId);
            Assert.Equal(product.Name, returnProduct.Name);
        }
        [Theory]
        [InlineData(1)]
        public void PutProduct_IdEqualProduct_ReturnNoContent(int productId)
        {
            Product product = new Product()
            {
                Id = 1,
                Name = "2",
                Color = "4",
                Stock = 4,
                Price = 32
            };
            var productFirst = products.First(x => x.Id == productId);
            Assert.Equal(product.Id, productFirst.Id);
            _mock.Setup(x => x.Update(productFirst));
            var result = _productApiController.PutProduct(productId, productFirst);
            _mock.Verify(X => X.Update(productFirst), Times.Once);
            Assert.IsType<NoContentResult>(result);
        }
        [Fact]
        public async void CreatedProduct_EqualProduct_ReturnCreated()
        {
            var product = products.First();
            _mock.Setup(x => x.Create(product));
            var result = await _productApiController.PostProduct(product);
            _mock.Verify(x => x.Create(product));
            var createdActionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal("GetProduct", createdActionResult.ActionName);
        }
        [Theory]
        [InlineData(99)]
        public async void DeleteProduct_NotEqualProductForId_ReturnBadRequest(int Id)
        {
            _mock.Setup(x => x.GetById(Id));
            var product = await _productApiController.DeleteProduct(Id);
            _mock.Verify(x => x.GetById(Id), Times.Once);
            var requestObjectResult = Assert.IsType<BadRequestObjectResult>(product);
            Assert.Equal("Product not find", requestObjectResult.Value);
        }
        [Theory]
        [InlineData(2)]
        public async void DeleteProduct_EqualProduct_ReturnNoContent(int Id)
        {
            var product = products.Find(x => x.Id == Id);
            _mock.Setup(x => x.GetById(Id)).ReturnsAsync(product);
            _mock.Setup(x => x.Delete(product));
            var objectResult = await _productApiController.DeleteProduct(product.Id);
            _mock.Verify(x => x.Delete(product), Times.Once);
            Assert.IsType<NoContentResult>(objectResult);

        }
        [Theory]
        [InlineData(2, 3, 5)]
        public void AddHelper_ReturnValue(int a, int b, int totalNumber)
        {
            var result = _helper.Add(a, b);
            Assert.Equal(result, totalNumber);
        }
    }
}
