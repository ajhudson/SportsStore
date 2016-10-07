using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsStore.Models
{
    public static class SeedData
    {
        public static void EnsurePopulated(IApplicationBuilder app)
        {
            ApplicationDbContext context = app.ApplicationServices.GetRequiredService<ApplicationDbContext>();

            if (!context.Products.Any())
            {
                context.Products.AddRange(
                    new Product { Name = "Kayak", Descripton = "A boat for one person", Category = "Watersports", Price = 275 },
                    new Product { Name = "Lifejacket", Descripton = "Protective and fashionable", Category = "Watersports", Price = 48.95m },
                    new Product { Name = "Soccer Ball", Descripton = "FIFA approved size and weight", Category = "Soccer", Price = 19.50m },
                    new Product { Name = "Corner Flags", Descripton = "Give your playing field a professional touch", Category = "Soccer", Price = 34.95m },
                    new Product { Name = "Stadium", Descripton = "Flat-packed 35,000 seat stadium", Category = "Soccer", Price = 79500 },
                    new Product { Name = "Thinking Cap", Descripton = "Improve brain efficiency by 75%", Category = "Chess", Price = 16},
                    new Product { Name = "Unsteady Chair", Descripton = "Secretly give your opponent a disadvantage", Category = "Chess", Price = 29.95m },
                    new Product { Name = "Human Chess Board", Descripton="A fun game for all the family", Category="Chess", Price = 75},
                    new Product { Name = "Bling-Bling King", Descripton="Gold plated, diamond studded King", Category="Chess", Price=1200 }
                );

                context.SaveChanges();
            }
        }

    }
}
