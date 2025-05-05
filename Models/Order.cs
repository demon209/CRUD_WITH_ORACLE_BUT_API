namespace MVC.Models
{
public class Order
{
    public required int OrderId {get; set;}
    public required int CustomerId {get; set;}
    public required DateTime OrderDate {get; set;}
    public required decimal TotalAmount {get; set;}
    public Customer? Customer { get; set; }
}
}
