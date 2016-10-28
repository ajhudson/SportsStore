using SportsStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SportsStore.Tests
{
    public class CartTests
    {
        [Fact]
        public void CanAddNewLines()
        {
            // arrange
            Product p1 = new Product { ProductId = 1, Name = "P1" };
            Product p2 = new Product { ProductId = 2, Name = "P2" };

            Cart cart = new Cart();

            // act
            cart.AddItem(p1, 1);
            cart.AddItem(p2, 1);

            CartLine[] results = cart.Lines.ToArray();

            // assert
            Assert.Equal(2, results.Length);
            Assert.Equal(p1, results[0].Product);
            Assert.Equal(p2, results[1].Product);
        }

        [Fact]
        public void CanddQuantityForExistingLines()
        {
            // arrange
            Product p1 = new Product { ProductId = 1, Name = "P1" };
            Product p2 = new Product { ProductId = 2, Name = "P2" };

            Cart cart = new Cart();

            // act
            cart.AddItem(p1, 1);
            cart.AddItem(p2, 1);
            cart.AddItem(p1, 10);

            CartLine[] results = cart.Lines.ToArray();

            // assert
            Assert.Equal(2, results.Length);
            Assert.Equal(11, results[0].Quantity);
            Assert.Equal(1, results[1].Quantity);
        }

        [Fact]
        public void CanRemoveLines()
        {
            // arrange
            Product p1 = new Product { ProductId = 1, Name = "P1" };
            Product p2 = new Product { ProductId = 2, Name = "P2" };
            Product p3 = new Product { ProductId = 3, Name = "P3" };

            Cart cart = new Cart();

            // act
            cart.AddItem(p1, 1);
            cart.AddItem(p2, 3);
            cart.AddItem(p3, 5);
            cart.AddItem(p2, 1);

            cart.RemoveLine(p2);

            // assert
            Assert.Equal(0, cart.Lines.Where(c => c.Product == p2).Count());
            Assert.Equal(2, cart.Lines.Count());
        }
        
        [Fact]
        public void CalculateCartTotal()
        {
            // arrange
            Product p1 = new Product { ProductId = 1, Name = "P1", Price = 100M };
            Product p2 = new Product { ProductId = 2, Name = "P2", Price = 50M };

            Cart cart = new Cart();

            // act
            cart.AddItem(p1, 1);
            cart.AddItem(p2, 1);
            cart.AddItem(p1, 3);

            decimal result = cart.ComputeTotalValue();

            // assert
            Assert.Equal(450M, result);
        }

        [Fact]
        public void CanClearContents()
        {
            // arrange
            Product p1 = new Product { ProductId = 1, Name = "P1", Price = 100M };
            Product p2 = new Product { ProductId = 2, Name = "P2", Price = 50M };

            // act
            Cart cart = new Cart();

            cart.AddItem(p1, 1);
            cart.AddItem(p2, 1);
            cart.Clear();

            // assert
            Assert.Equal(0, cart.Lines.Count());
        }
    }
}
