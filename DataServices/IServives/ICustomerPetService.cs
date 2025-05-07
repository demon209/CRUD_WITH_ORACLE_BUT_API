using MVC.Models;
using System.Collections.Generic;

namespace MVC.Services
{
    public interface ICustomerPetService
    {
        List<CustomerPet> GetAllCustomerPets();
        List<CustomerPet> SearchCustomerPets(string keyword);
        CustomerPet GetCustomerPetById(int id);
        string AddCustomerPet(CustomerPet customerpet);
        string UpdateCustomerPet(CustomerPet customerpet);
        string DeleteCustomerPet(int customerpetId);
    }
}
