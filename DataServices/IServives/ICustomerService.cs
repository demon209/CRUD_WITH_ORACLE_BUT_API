using MVC.Models;
using System.Collections.Generic;

namespace MVC.Services
{
    public interface ICustomerService : ICrudService<Customer>
    {
        List<Customer> SearchCustomers(string keyword);
    }
}
