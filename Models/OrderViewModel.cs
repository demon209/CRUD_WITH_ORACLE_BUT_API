public class OrderViewModel
{
    public int OrderId { get; set; }
    public int CustomerId { get; set; }
    public string LastName { get; set; }
    public string FirstName { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal TotalAmount { get; set; }

    public int? PetId { get; set; }
    public string? PetName { get; set; }

    public int? ProductId { get; set; }
    public string? ProductName { get; set; }

    public int? Quantity { get; set; }
}
