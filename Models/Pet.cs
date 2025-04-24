namespace MVC.Models
{
public class Pet
{
    public required string PetName { get; set; }
    public required string PetType { get; set; }
    public required string Breed { get; set; }
    public required string Gender { get; set; }
    public int PetId { get; set; }
    public int Age { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }
}

}
