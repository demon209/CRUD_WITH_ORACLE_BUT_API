using MVC.Models;
using System.Collections.Generic;

namespace MVC.Services
{
    public interface IProductService : ICrudService<Product>
    {
        List<Product> SearchProducts(string keyword);

    }
}

