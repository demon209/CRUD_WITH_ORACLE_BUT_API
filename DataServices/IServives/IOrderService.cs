using MVC.Models;
using System.Collections.Generic;

namespace MVC.Services
{
    public interface IOrderService
    {
        List<Order> GetAllOrders();
        List<Order> SearchOrders(string keyword);
        Order GetOrderById(int id);
        string AddOrder(Order order);
        string UpdateOrder(Order order);
        string DeleteOrder(int orderId);
    }
}
