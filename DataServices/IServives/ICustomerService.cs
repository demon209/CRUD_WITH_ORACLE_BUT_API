using MVC.Models;
using System.Collections.Generic;

namespace MVC.Services
{
    public interface ICustomerService
    {
        List<Customer> GetAllCustomers();
        List<Customer> SearchCustomers(string keyword);
        Customer GetCustomerById(int id);
        string AddCustomer(Customer customer);
        string UpdateCustomer(Customer customer);
        string DeleteCustomer(int customerId);
    }
}
