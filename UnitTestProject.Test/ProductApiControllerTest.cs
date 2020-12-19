using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnitTestProject.Web.Controllers;
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
        public ProductApiControllerTest()
        {
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
            Assert.IsType<NotFoundResult>(result);
        }
        [Theory]
        [InlineData(2)]
        [InlineData(3)]
        public async void GetProducy_Invalid_ReturnOkResult(int productId)
        {
            var product = products.First(x => x.Id == productId);
            _mock.Setup(x => x.GetById(productId)).ReturnsAsync(product);
            var result = await _productApiController.GetProduct(productId);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnProduct = Assert.IsType<Product>(okResult.Value);
            Assert.Equal<int>(product.Id, productId);
            Assert.Equal(product.Name, returnProduct.Name);
        }
    }
}
