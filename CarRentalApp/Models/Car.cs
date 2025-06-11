using System.ComponentModel.DataAnnotations;

namespace CarRentalApp.Models
{
    public class Car
    {
        public int Id { get; set; }
        [Required]
        public string Make { get; set; }
        [Required]
        public string Model { get; set; }
        [Required]
        public string Year { get; set; }
        [Range(0, 1000)]
        public decimal PricePerDay { get; set; }
        public bool IsAvailable { get; set; } = true;
        public DateTime? RentedUntil { get; set; }

        public ICollection<CarImage> Images { get; set; } = new List<CarImage>();

    }
}
