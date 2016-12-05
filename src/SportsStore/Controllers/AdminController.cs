using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsStore.Controllers
{
    public class AdminController : Controller 
    {
        private IProductRepository repo;

        public AdminController(IProductRepository repo)
        {
            this.repo = repo;
        }

        public ViewResult Create() => View("Edit", new Product());

        public ViewResult Index() => View(repo.Products);

        public ViewResult Edit(int productId) => View(repo.Products.FirstOrDefault(p => p.ProductId == productId));

        [HttpPost]
        public IActionResult Edit(Product product)
        {
            if (this.ModelState.IsValid)
            {
                repo.SaveProduct(product);
                TempData["message"] = $"{product.Name} has been saved";
                return RedirectToAction("Index");
            }
            else
            {
                return View(product);
            }
        }
    }
}
