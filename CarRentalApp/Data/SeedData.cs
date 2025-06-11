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

             //ResetTestData(context);

            if (context.Cars.Any()) return; // Don't seed if already present

            AddCarWithImages(context, "BMW", "X5", "2015", 120);
            AddCarWithImages(context, "Audi", "A6", "2018", 100);
            AddCarWithImages(context, "Tesla", "Model 3", "2020", 150);

            context.SaveChanges();
        }

        private static void AddCarWithImages(ApplicationDbContext context, string make, string model, string year, decimal price)
        {
            var car = new Car
            {
                Make = make,
                Model = model,
                Year = year,
                PricePerDay = price,
                IsAvailable = true,
                Images = new List<CarImage>()
            };

            for (int i = 1; i <= 6; i++)
            {
                string path = $"ImagesForTesting/{make} {model}/{i}.jpg";
                var imageData = LoadImageBytes(path);

                if (imageData != null)
                {
                    car.Images.Add(new CarImage
                    {
                        FileName = $"{i}.jpg",
                        ImageData = imageData
                    });
                }
            }

            context.Cars.Add(car);
        }

        private static byte[]? LoadImageBytes(string relativePath)
        {
            var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", relativePath.Replace("/", Path.DirectorySeparatorChar.ToString()));
            return File.Exists(fullPath) ? File.ReadAllBytes(fullPath) : null;
        }

        public static void ResetTestData(ApplicationDbContext context)
        {
            context.CarImages.RemoveRange(context.CarImages);
            context.CarRentals.RemoveRange(context.CarRentals);
            context.Cars.RemoveRange(context.Cars);
            context.SaveChanges();
        }
    }
}
