using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;
using SportsStore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SportsStore.Models.ViewModels;

namespace SportsStore.Controllers
{
    public class CartController : Controller 
    {
        private IProductRepository repository;
        private Cart cart;

        public CartController(IProductRepository repository, Cart cartService)
        {
            this.repository = repository;
            this.cart = cartService;
        }

        public ViewResult Index(string returnUrl)
        {
            var model = new CartIndexViewModel { Cart = cart, ReturnUrl = returnUrl };
            return View(model);
        }

        public RedirectToActionResult AddToCart(int productId, string returnUrl)
        {
            Product product = repository.Products.FirstOrDefault(p => p.ProductId == productId);

            if (product != null)
            {
                if (cart == null)
                {
                    cart = new Cart();
                }

                cart.AddItem(product, 1);
            }

            return RedirectToAction("Index", new { returnUrl });
        }

        public RedirectToActionResult RemoveFromCart(int productId, string returnUrl)
        {
            Product product = repository.Products.FirstOrDefault(p => p.ProductId == productId);

            if (product != null)
            {
                cart.RemoveLine(product);
            }

            return RedirectToAction("Index", new { returnUrl });
        }
    }
}
