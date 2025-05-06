using MVC.Models;
using System.Collections.Generic;

namespace MVC.Services
{
    public interface IPetService
    {
        List<Pet> GetAllPets();
        List<Pet> SearchPets(string keyword);
        Pet GetPetById(int id);
        string AddPet(Pet pet);
        string UpdatePet(Pet pet);
        string DeletePet(int petid);
    }
}
