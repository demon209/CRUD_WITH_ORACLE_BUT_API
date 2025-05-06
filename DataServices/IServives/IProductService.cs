using MVC.Models;
using System.Collections.Generic;

namespace MVC.Services
{
    public interface IProductService
    {
        List<Product> GetAllProducts();
        List<Product> SearchProducts(string keyword);
        Product GetProductById(int id);
        string AddProduct(Product product);
        string UpdateProduct(Product product);
        string DeleteProduct(int productid);
    }
}
