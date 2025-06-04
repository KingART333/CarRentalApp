using CarRentalApp.Models;
using CarRentalApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CarRentalApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly CarService _carService;

        public IndexModel(ILogger<IndexModel> logger, CarService carService)
        {
            _logger = logger;
            _carService = carService;
        }

        public List<Car> Cars { get; set; }

        public void OnGet()
        {
            Cars = _carService.GetAllCars();
        }
    }
}
