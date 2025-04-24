namespace MVC.Models
{
public class Product
{
    public required int ProductId {get;set;}
    public required string ProductName {get; set;}
    public required string ProductCategory {get; set;}
    public required decimal ProductPrice {get; set;}
    public required int ProductStock {get; set;}
}

}