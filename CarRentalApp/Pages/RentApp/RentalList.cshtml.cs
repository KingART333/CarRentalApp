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

        [BindProperty(SupportsGet = true)]
        public string Filter { get; set; } = string.Empty;

        public async Task OnGetAsync()
        {
            var query = _context.CarRentals.Include(r => r.Car).AsQueryable();

            switch (Filter)
            {
                case "Id":
                    query = query.OrderBy(r => r.Id);
                    break;
                case "Name":
                    query = query.OrderBy(r => r.UserName);
                    break;
                case "Car Id":
                    query = query.OrderBy(r => r.CarId);
                    break;
                case "Car Make":
                    query = query.OrderBy(r => r.Car.Make);
                    break;
                case "Car Model":
                    query = query.OrderBy(r => r.Car.Model);
                    break;
                case "Rent Date":
                    query = query.OrderBy(r => r.RentDate).ThenBy(r => r.UserName);
                    break;
                case "Return Date":
                    query = query.OrderBy(r => r.ReturnDate).ThenBy(r => r.UserName); ;
                    break;
                default:
                    query = query.OrderBy(r => r.Id);
                    break;
            }

            int totalRecords = await query.CountAsync();
            TotalPages = (int)Math.Ceiling(totalRecords / (double)PageSize);

            Rentals = await query
                .Skip((PageNumber - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();
        }
    }
}
