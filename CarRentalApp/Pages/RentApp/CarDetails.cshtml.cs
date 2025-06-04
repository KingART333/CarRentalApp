using CarRentalApp.Data;
using CarRentalApp.Models;
using CarRentalApp.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CarRentalApp.Pages.RentApp
{
    public class CarDetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public CarDetailsModel(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public Car SelectedCar { get; set; }

        [BindProperty]
        public int Days { get; set; }

        public void OnGet(int id)
        {
            SelectedCar = _context.Cars.FirstOrDefault(c => c.Id == id);
        }


        public IActionResult OnPostRent(int id)
        {
            SelectedCar = _context.Cars.FirstOrDefault(c => c.Id == id);

            if (SelectedCar == null || !SelectedCar.IsAvailable)
            {
                return RedirectToPage("/Error");
            }

            var userId = _userManager.GetUserId(User);
            if (userId == null)
            {
                return Challenge(); // Redirect to login if not authenticated
            }

            var rental = new CarRental
            {
                UserId = userId,
                CarId = SelectedCar.Id,
                RentDate = DateTime.Today,
                ReturnDate = DateTime.Today.AddDays(Days)
            };

            SelectedCar.IsAvailable = false;
            SelectedCar.RentedUntil = rental.ReturnDate;

            _context.CarRentals.Add(rental);
            _context.SaveChanges();

            TempData["Message"] = $"You rented the car for {Days} days. Total price: ${Days * SelectedCar.PricePerDay}";
            return RedirectToPage(new { id = id });
        }

    }
}
