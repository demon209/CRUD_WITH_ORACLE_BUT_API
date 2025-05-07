namespace MVC.Models
{
    public class CustomerPet
    {
        public required int CustomerPetId { get; set; }
        public required int CustomerId { get; set; }
        public required int PetId { get; set; }
        public required DateTime PurchaseDate { get; set; }
        public required decimal PriceAtPurchase { get; set; }

        
        // Foreign key
        public Customer Customer { get; set; } = null!;
        public Pet Pet { get; set; } = null!;

    }
}
