using CarRentalApp.Models;

namespace CarRentalApp.Services
{
    public class CarService
    {
        private static readonly List<Car> _cars = new List<Car>
    {
        new Car { Id = 1, Make = "BMW", Model = "X5", Year = "2015", ImageUrl = "/ImagesForTesting/BMW X5/6.jpg", PricePerDay = 120, IsAvailable = true },
        new Car { Id = 2, Make = "Audi", Model = "A6", Year = "2018", ImageUrl = "/ImagesForTesting/Audi A6/6.jpg", PricePerDay = 100, IsAvailable = true },
        new Car { Id = 3, Make = "Tesla", Model = "Model 3", Year = "2020", ImageUrl = "/ImagesForTesting/Tesla model 3.jpg", PricePerDay = 150, IsAvailable = true }
    };

        private void RefreshAvailability()
        {
            foreach (var car in _cars)
            {
                if (car.RentedUntil.HasValue && car.RentedUntil.Value <= DateTime.Today)
                {
                    car.IsAvailable = true;
                    car.RentedUntil = null;
                }
            }
        }

        public List<Car> GetAllCars()
        {
            RefreshAvailability();
            return _cars;
        }

        public Car GetCarById(int id)
        {
            RefreshAvailability();
            return _cars.FirstOrDefault(c => c.Id == id);
        }

        public void UpdateCar(Car car)
        {
            var existing = _cars.FirstOrDefault(c => c.Id == car.Id);
            if (existing != null)
            {
                existing.RentedUntil = car.RentedUntil;
                existing.IsAvailable = car.IsAvailable;
            }
        }
    }
}