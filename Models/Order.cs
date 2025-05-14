namespace MVC.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public int? PetId { get; set; }
        public int? ProductId { get; set; }
        public int? ProductQuantity {get; set;}
        public  DateTime OrderDate { get; set; }
        public  decimal TotalAmount { get; set; }

        public Customer? Customer { get; set; }
        public Pet? Pet { get; set; }
        public Product? Product { get; set; }
        
    }
}
