using CarRentalApp.Data;
using CarRentalApp.Models;
using Microsoft.EntityFrameworkCore;

namespace CarRentalApp
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>());

            // DEV RESET: Clear rental state for testing
            //foreach (var car in context.Cars)
            //{
            //    car.IsAvailable = true;
            //    car.RentedUntil = null;
            //}
            //context.SaveChanges();

            if (context.Cars.Any()) return; // DB already seeded

            context.Cars.AddRange(
                new Car { Id = 1, Make = "BMW", Model = "X5", Year = "2015", ImageUrl = "/ImagesForTesting/BMW X5/6.jpg", PricePerDay = 120, IsAvailable = true },
                new Car { Id = 2, Make = "Audi", Model = "A6", Year = "2018", ImageUrl = "/ImagesForTesting/Audi A6/6.jpg", PricePerDay = 100, IsAvailable = true },
                new Car { Id = 3, Make = "Tesla", Model = "Model 3", Year = "2020", ImageUrl = "/ImagesForTesting/Tesla model 3.jpg", PricePerDay = 150, IsAvailable = true }
            );

            context.SaveChanges();
        }
    }
}