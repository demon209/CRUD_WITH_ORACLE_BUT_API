using MVC.Models; 
public class OrderDetailViewModel
{
    public required Order Order { get; set; }
    public required Customer Customer { get; set; }
    public Pet? Pet { get; set; }
    public Product? Product { get; set; }
}


