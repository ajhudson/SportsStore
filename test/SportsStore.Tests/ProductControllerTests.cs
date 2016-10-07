using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Moq;
using SportsStore.Models;
using SportsStore.Controllers;
using SportsStore.Models.ViewModels;

namespace SportsStore.Tests
{
    public class ProductControllerTests
    {
        [Fact]
        public void CanPaginate()
        {
            // Arrange
            List<Product> products = new List<Product>
            {
                new Product { ProductId = 1, Name = "P1" },
                new Product { ProductId = 2, Name = "P2" },
                new Product { ProductId = 3, Name = "P3" },
                new Product { ProductId = 4, Name = "P4" },
                new Product { ProductId = 5, Name = "P5" }
            };

            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(products);

            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;

            // Act
            var result = controller.List(null, 2).ViewData.Model as ProductsListViewModel;

            // Assert
            Product[] prod = result.Products.ToArray();
            Assert.Equal(2, prod.Length);
            Assert.Equal("P4", prod[0].Name);
            Assert.Equal("P5", prod[1].Name);
        }

        [Fact]
        public void CanSendPaginationViewModel()
        {
            // arrange
            List<Product> products = new List<Product>
            {
                new Product { ProductId = 1, Name = "P1" },
                new Product { ProductId = 2, Name = "P2" },
                new Product { ProductId = 3, Name = "P3" },
                new Product { ProductId = 4, Name = "P4" },
                new Product { ProductId = 5, Name = "P5" }
            };

            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(products);

            ProductController controller = new ProductController(mock.Object) { PageSize = 3 };

            // act
            ProductsListViewModel result = controller.List(null, 2).ViewData.Model as ProductsListViewModel;

            // assert
            PagingInfo pageInfo = result.PagingInfo;
            Assert.Equal(2, pageInfo.CurrentPage);
            Assert.Equal(3, pageInfo.ItemsPerPage);
            Assert.Equal(5, pageInfo.TotalItems);
            Assert.Equal(2, pageInfo.TotalPages);
        }

        [Fact]
        public void CanFilterProducts()
        {
            // arrange
            List<Product> products = new List<Product>
            {
                new Product { ProductId = 1, Name = "P1", Category = "Cat1" },
                new Product { ProductId = 2, Name = "P2", Category = "Cat2" },
                new Product { ProductId = 3, Name = "P3", Category = "Cat1" },
                new Product { ProductId = 4, Name = "P4", Category = "Cat2" },
                new Product { ProductId = 5, Name = "P5", Category = "Cat3" }
            };

            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(products);

            var controller = new ProductController(mock.Object);
            controller.PageSize = 3;

            // act
            var result = controller.List("Cat2", 1).ViewData.Model as ProductsListViewModel;
            Product[] filteredProducts = result.Products.ToArray();

            // assert
            Assert.Equal(2, filteredProducts.Count());
            Assert.True(filteredProducts[0].Category == "Cat2" && filteredProducts[0].Name == "P2");
            Assert.True(filteredProducts[1].Category == "Cat2" && filteredProducts[1].Name == "P4");
        }
    }
}
