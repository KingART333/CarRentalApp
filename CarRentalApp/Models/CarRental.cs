namespace CarRentalApp.Models
{
    public class CarRental
    {
        public int Id { get; set; }

        public string UserId { get; set; }
        public int CarId { get; set; }

        public DateTime RentDate { get; set; }
        public DateTime? ReturnDate { get; set; }
    }
}
