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
        public string Sort { get; set; } = string.Empty;

        [BindProperty(SupportsGet = true)]
        public string SearchId { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchUserName { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchCarId { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchMake { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchModel { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime? SearchRentDate { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime? SearchReturnDate { get; set; }

        public async Task OnGetAsync()
        {
            var query = _context.CarRentals.Include(r => r.Car).AsQueryable();

            if (!string.IsNullOrEmpty(SearchId) && int.TryParse(SearchId, out int id))
                query = query.Where(r => r.Id == id);

            if (!string.IsNullOrEmpty(SearchUserName))
                query = query.Where(r => r.UserName.ToLower().Contains(SearchUserName.ToLower()));

            if (!string.IsNullOrEmpty(SearchCarId) && int.TryParse(SearchCarId, out int carId))
                query = query.Where(r => r.CarId == carId);

            if (!string.IsNullOrEmpty(SearchMake))
                query = query.Where(r => r.Car.Make.ToLower().Contains(SearchMake.ToLower()));

            if (!string.IsNullOrEmpty(SearchModel))
                query = query.Where(r => r.Car.Model.ToLower().Contains(SearchModel.ToLower()));

            if (SearchRentDate != default(DateTime) && DateTime.TryParse(SearchRentDate.ToString(), out DateTime rentDate))
                query = query.Where(r => r.RentDate.Date == rentDate.Date);

            if (SearchReturnDate != default(DateTime) && DateTime.TryParse(SearchReturnDate.ToString(), out DateTime returnDate))
                query = query.Where(r => r.ReturnDate.HasValue && r.ReturnDate.Value.Date == returnDate.Date);

            switch (Sort)
            {
                case "Id":
                    query = query.OrderBy(r => r.Id);
                    break;
                case "Name":
                    //query = query.OrderBy(r => r.UserName);
                    break;
                case "CarId":
                    query = query.OrderBy(r => r.CarId);
                    break;
                case "CarMake":
                    query = query.OrderBy(r => r.Car.Make);
                    break;
                case "CarModel":
                    query = query.OrderBy(r => r.Car.Model);
                    break;
                case "RentDate":
                    query = query.OrderBy(r => r.RentDate);
                    break;
                case "ReturnDate":
                    query = query.OrderBy(r => r.ReturnDate);
                    break;
                default:
                    query = query.OrderBy(r => r.Id);
                    break;
            }

            int totalRecords = await query.CountAsync();
            TotalPages = (int)Math.Ceiling(totalRecords / (double)PageSize);

            var result = await query
                .Skip((PageNumber - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();

            if (Sort == "Name")
            {
                result = result.OrderBy(r => SortByName(r.UserName)).ToList();
            }

            Rentals = result;
        }

        private string SortByName(string userName)
        {
            if (string.IsNullOrEmpty(userName))
                return string.Empty;

            var index = userName.IndexOf('@');
            if (index > 0)
                return userName.Substring(0, index).ToLowerInvariant().Trim();

            return userName.ToLowerInvariant().Trim();
        }
    }
}
