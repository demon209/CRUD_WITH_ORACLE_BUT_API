using MVC.Models;
using System.Collections.Generic;

namespace MVC.Services
{
    public interface IPetService : ICrudService<Pet>
    {
        List<Pet> SearchPets(string keyword);
        string Add(Pet pet, byte[]? imageData);      // overload
        string Update(Pet pet, byte[]? imageData);   // overload
    }
}
