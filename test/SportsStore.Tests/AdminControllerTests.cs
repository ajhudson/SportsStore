using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using SportsStore.Controllers;
using SportsStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SportsStore.Tests
{
    public class AdminControllerTests
    {
        [Fact]
        public void IndexContainsAllProducts()
        {
            // arrange
            Product[] products = new Product[]
            {
                new Product { ProductId = 1, Name = "P1" },
                new Product { ProductId = 2, Name = "P2" },
                new Product { ProductId = 3, Name = "P3" }
            };

            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(x => x.Products).Returns(products);

            AdminController controller = new AdminController(mock.Object);

            // act
            Product[] result = GetViewModel<IEnumerable<Product>>(controller.Index())?.ToArray();

            // assert
            Assert.Equal(3, result.Length);
            Assert.Equal("P1", result[0].Name);
            Assert.Equal("P2", result[1].Name);
            Assert.Equal("P3", result[2].Name);
        }

        [Fact]
        public void CanEditProduct()
        {
            // arrange
            Product[] products = new Product[]
            {
                new Product { ProductId = 1, Name = "P1" },
                new Product { ProductId = 2, Name = "P2" },
                new Product { ProductId = 3, Name = "P3" }
            };

            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(x => x.Products).Returns(products);

            AdminController controller = new AdminController(mock.Object);

            // act
            Product p1 = GetViewModel<Product>(controller.Edit(1));
            Product p2 = GetViewModel<Product>(controller.Edit(2));
            Product p3 = GetViewModel<Product>(controller.Edit(3));

            // assert
            Assert.Equal(1, p1.ProductId);
            Assert.Equal(2, p2.ProductId);
            Assert.Equal(3, p3.ProductId);
        }

        [Fact]
        public void CannotEditNonExistentProduct()
        {
            // arrange
            Product[] products = new Product[]
            {
                new Product { ProductId = 1, Name = "P1" },
                new Product { ProductId = 2, Name = "P2" },
                new Product { ProductId = 3, Name = "P3" }
            };

            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(x => x.Products).Returns(products);

            AdminController controller = new AdminController(mock.Object);

            // act
            Product result = GetViewModel<Product>(controller.Edit(99));

            // assert
            Assert.Null(result);
        }

        [Fact]
        public void CanSaveValidChanges()
        {
            // arrange 
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            Mock<ITempDataDictionary> tempData = new Mock<ITempDataDictionary>();
            AdminController controller = new AdminController(mock.Object) { TempData = tempData.Object };
            Product product = new Product { Name = "Test" };

            // act 
            IActionResult result = controller.Edit(product);

            // assert
            mock.Verify(x => x.SaveProduct(product));
            Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", (result as RedirectToActionResult).ActionName);
        }

        [Fact]
        public void CannotSaveInvalidChanges()
        {
            // arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            AdminController controller = new AdminController(mock.Object);
            Product product = new Product { Name = "Test" };
            controller.ModelState.AddModelError("error", "error");

            // act 
            IActionResult result = controller.Edit(product);

            // assert
            mock.Verify(m => m.SaveProduct(It.IsAny<Product>()), Times.Never);
            Assert.IsType<ViewResult>(result);
        }

        private T GetViewModel<T>(IActionResult result) where T : class
        {
            return (result as ViewResult)?.ViewData.Model as T;
        }
    }
}
