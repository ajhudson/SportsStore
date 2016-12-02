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

        public ViewResult Index() => View(repo.Products);

        public ViewResult Edit(int productId) => View(repo.Products.FirstOrDefault(p => p.ProductId == productId));
    }
}
