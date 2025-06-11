using CarRentalApp.Data;
using CarRentalApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CarRentalApp.Pages.RentApp
{
    public class RentalListModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public RentalListModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<CarRental> Rentals { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; } = 1;

        public int TotalPages { get; set; }
        public int PageSize { get; set; } = 10;

        public async Task OnGetAsync()
        {
            int totalRecords = await _context.CarRentals.CountAsync();
            TotalPages = (int)Math.Ceiling(totalRecords / (double)PageSize);

            Rentals = await _context.CarRentals
                .Include(r => r.Car)
                .OrderBy(r => r.Id)
                .Skip((PageNumber - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();
        }
    }
}
