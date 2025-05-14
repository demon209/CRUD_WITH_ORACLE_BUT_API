using MVC.Models;
using System.Collections.Generic;

namespace MVC.Services
{
    public interface ICustomerPetService : ICrudService<CustomerPet>
    {
        List<CustomerPet> SearchCustomerPet(string keyword);
        string ToggleStatus(int id); 
    }
}

