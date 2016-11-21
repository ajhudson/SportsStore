using Microsoft.AspNetCore.Mvc;
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
    public class OrderControllerTests
    {
        [Fact]
        public void CannotCheckoutEmptyCart()
        {
            // arrange
            Mock<IOrderRepository> mock = new Mock<IOrderRepository>();
            Cart cart = new Cart();
            Order order = new Order();
            OrderController controller = new OrderController(mock.Object, cart);

            // act 
            ViewResult result = controller.Checkout(order) as ViewResult;

            // assert
            mock.Verify(m => m.SaveOrder(It.IsAny<Order>()), Times.Never);
            Assert.True(string.IsNullOrEmpty(result.ViewName));
            Assert.False(result.ViewData.ModelState.IsValid);
        }

        [Fact]
        public void CannotCheckoutInvalidShippingDetails()
        {
            // arrange
            Mock<IOrderRepository> mock = new Mock<IOrderRepository>();

            Cart cart = new Cart();
            cart.AddItem(new Product(), 1);

            OrderController controller = new OrderController(mock.Object, cart);
            controller.ModelState.AddModelError("error", "error");

            // act
            ViewResult result = controller.Checkout(new Order()) as ViewResult;

            // assert
            mock.Verify(m => m.SaveOrder(It.IsAny<Order>()), Times.Never);
            Assert.True(string.IsNullOrEmpty(result.ViewName));
            Assert.False(result.ViewData.ModelState.IsValid);
        }

        [Fact]
        public void CanCheckoutAndSubmitOrder()
        {
            // arrange
            Mock<IOrderRepository> mock = new Mock<IOrderRepository>();
            Cart cart = new Cart();
            cart.AddItem(new Product(), 1);
            OrderController controller = new OrderController(mock.Object, cart);

            // act 
            RedirectToActionResult result = controller.Checkout(new Order()) as RedirectToActionResult;

            // assert
            mock.Verify(m => m.SaveOrder(It.IsAny<Order>()), Times.Once);
            Assert.Equal("Completed", result.ActionName);
        }
    }
}
