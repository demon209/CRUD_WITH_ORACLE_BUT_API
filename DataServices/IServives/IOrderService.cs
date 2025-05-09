using MVC.Models;
using System.Collections.Generic;

namespace MVC.Services
{
    public interface IOrderService : ICrudService<Order>
    {
        List<Order> SearchOrders(string keyword);
    }
}
