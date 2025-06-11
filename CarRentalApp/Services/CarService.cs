using CarRentalApp.Data;
using CarRentalApp.Models;

namespace CarRentalApp.Services
{
    public class CarService
    {
        //private readonly ApplicationDbContext _context;

        //public CarService(ApplicationDbContext context)
        //{
        //    _context = context;
        //}

        //public List<Car> GetAllCars()
        //{
        //    RefreshAvailability();
        //    return _context.Cars.ToList();
        //}

        //public Car GetCarById(int id)
        //{
        //    RefreshAvailability();
        //    return _context.Cars.FirstOrDefault(c => c.Id == id);
        //}

        //public void UpdateCar(Car car)
        //{
        //    var existing = _context.Cars.FirstOrDefault(c => c.Id == car.Id);
        //    if (existing != null)
        //    {
        //        existing.RentedUntil = car.RentedUntil;
        //        existing.IsAvailable = car.IsAvailable;
        //        _context.SaveChanges();
        //    }
        //}

        //private void RefreshAvailability()
        //{
        //    foreach (var car in _context.Cars)
        //    {
        //        if (car.RentedUntil.HasValue && car.RentedUntil.Value <= DateTime.Today)
        //        {
        //            car.IsAvailable = true;
        //            car.RentedUntil = null;
        //        }
        //    }
        //    _context.SaveChanges();
        //}
    }
}