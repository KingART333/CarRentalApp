using CarRentalApp.Data;
using CarRentalApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CarRentalApp.Pages.RentApp
{
    [Authorize]
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
            SelectedCar = _context.Cars
                .Include(c => c.Images)
                .FirstOrDefault(c => c.Id == id);

            if (SelectedCar != null && SelectedCar.RentedUntil <= DateTime.Today)
            {
                SelectedCar.IsAvailable = true;
                SelectedCar.RentedUntil = null;
                _context.SaveChanges();
            }
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
                return Challenge(); // Redirect to login
            }

            if (ToDate <= FromDate)
            {
                TempData["Message"] = "Invalid rental period.";
                return RedirectToPage(new { id = id });
            }

            int days = (ToDate - FromDate).Days;

            var rental = new CarRental
            {
                UserId = userId,
                CarId = SelectedCar.Id,
                UserName = _userManager.GetUserName(User),
                RentDate = FromDate,
                ReturnDate = ToDate
            };

            SelectedCar.IsAvailable = false;
            SelectedCar.RentedUntil = rental.ReturnDate;

            _context.CarRentals.Add(rental);
            _context.SaveChanges();

            TempData["Message"] = $"You rented the car for {days} day(s). Total price: ${days * SelectedCar.PricePerDay}";
            return RedirectToPage(new { id = id });
        }


    }
}
