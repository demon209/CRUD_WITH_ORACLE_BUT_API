using System.Collections.Generic;

namespace MVC.Services
{
    public interface ICrudService<T>
    {
        List<T> GetAll();
        T GetById(int id);
        string Add(T entity);
        string Update(T entity);
        string Delete(int id);
    }
}
