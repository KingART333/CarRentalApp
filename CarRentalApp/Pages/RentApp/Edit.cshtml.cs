using CarRentalApp.Data;
using CarRentalApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CarRentalApp.Pages.RentApp
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public EditModel(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public Car NewCar { get; set; }
        [BindProperty]
        public List<IFormFile> Upload { get; set; }

        [BindProperty]
        public string ErrorMessage { get; set; }

        public IActionResult OnGet(int? id)
        {
            if (id == null)
            {
                NewCar = new Car();
            }
            else
            {
                NewCar = _context.Cars.FirstOrDefault(c => c.Id == id);

                if (NewCar == null)
                {
                    return RedirectToPage("/NotFound");
                }
            }

            return Page();
        }


        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var userId = _userManager.GetUserId(User);
            if (userId == null)
            {
                return Challenge(); // Redirect to login
            }

            if (NewCar.Id == 0)
            {
                _context.Cars.Add(NewCar);
            }
            else
            {
                _context.Cars.Update(NewCar);
            }

            if (Upload != null && Upload.Any())
            {
                if (NewCar.Images == null)
                {
                    NewCar.Images = new List<CarImage>();
                }
                foreach (var file in Upload)
                {
                    if (file.Length > 0)
                    {
                        using var ms = new MemoryStream();
                        await file.CopyToAsync(ms);

                        NewCar.Images.Add(new CarImage
                        {
                            FileName = file.FileName,
                            ImageData = ms.ToArray()
                        });
                    }
                }
            }

            await _context.SaveChangesAsync();

            return RedirectToPage("/Index");
        }


        public async Task<IActionResult> OnPostDelete()
        {
            var car = _context.Cars.FirstOrDefault(c => c.Id == NewCar.Id);

            if (car == null)
            {
                ErrorMessage = $"Car with ID {NewCar.Id} not found.";
                return Page();
            }

            _context.Cars.Remove(car);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Index");
        }
    }
}
