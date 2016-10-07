using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.AspNetCore.Routing;
using Moq;
using SportsStore.Components;
using SportsStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SportsStore.Tests
{
    public class NavigationMenuViewComponentTests
    {
        [Fact]
        public void CanSelectCategories()
        {
            // arrange
            List<Product> products = new List<Product>
            {
                new Product { ProductId = 1, Name = "P1", Category = "Apples" },
                new Product { ProductId = 2, Name = "P2", Category = "Apples" },
                new Product { ProductId = 3, Name = "P3", Category = "Plums" },
                new Product { ProductId = 4, Name = "P4", Category = "Oranges" },
            };

            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(x => x.Products).Returns(products);

            NavigationMenuViewComponent target = new NavigationMenuViewComponent(mock.Object);

            // act
            string[] results = ((IEnumerable<string>)(target.Invoke() as ViewViewComponentResult).ViewData.Model).ToArray();

            // assert
            Assert.True(Enumerable.SequenceEqual(new string[] { "Apples", "Oranges", "Plums" }, results));
        }

        [Fact]
        public void IndicatesSelectedCategory()
        {
            // arrange
            string categoryToSelect = "Apples";

            List<Product> products = new List<Product>
            {
                new Product { ProductId = 1, Name = "P1", Category = "Apples" },
                new Product { ProductId = 4, Name = "P2", Category = "Oranges" }
            };

            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(x => x.Products).Returns(products);

            NavigationMenuViewComponent target = new NavigationMenuViewComponent(mock.Object);
            target.ViewComponentContext = new ViewComponentContext
            {
                ViewContext = new ViewContext
                {
                    RouteData = new RouteData()
                }
            };

            target.RouteData.Values["category"] = categoryToSelect;

            // act
            string result = (string)(target.Invoke() as ViewViewComponentResult).ViewData["SelectedCategory"];

            // assert
            Assert.Equal(categoryToSelect, result);
        }
    }
}
