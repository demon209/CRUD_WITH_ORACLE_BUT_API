using MVC.Models;
using System.Collections.Generic;

namespace MVC.Services
{
    public interface IPetService
    {
        List<Pet> GetAllPets();
        List<Pet> SearchPets(string keyword);
        Pet GetPetById(int id);
        string AddPet(Pet pet, byte[] imageData);     // phải trùng
        string UpdatePet(Pet pet, byte[]? imageData);  // phải trùng
        string DeletePet(int id);
    }
}
