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
        public DateTime FromDate { get; set; }

        [BindProperty]
        public DateTime ToDate { get; set; }

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
                return Challenge(); // User must be logged in
            }

            if (FromDate > ToDate)
            {
                ModelState.AddModelError("", "Return date must be after or equal to rent date.");
                return Page();
            }

            int rentalDays = (ToDate - FromDate).Days + 1;

            var rental = new CarRental
            {
                UserId = userId,
                CarId = SelectedCar.Id,
                RentDate = FromDate,
                ReturnDate = ToDate
            };
            SelectedCar.IsAvailable = false;
            SelectedCar.RentedUntil = ToDate;

            _context.CarRentals.Add(rental);
            _context.SaveChanges();

            TempData["Message"] = $"You rented the car from {FromDate:dd MMM} to {ToDate:dd MMM}. Total price: ${rentalDays * SelectedCar.PricePerDay}";
            return RedirectToPage(new { id = id });
        }


    }
}
