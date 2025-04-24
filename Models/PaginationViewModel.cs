using System;
using System.Collections.Generic;
using MVC.Models;

public class PaginationViewModel
{
    public int CurrentPage { get; set; }
    public int CountPages { get; set; }
    public Func<int?, string> GenerateUrl { get; set; }
}

public class PetListViewModel : PaginationViewModel
{
    public List<Pet> Pets { get; set; }
}

public class CustomerListViewModel : PaginationViewModel
{
    public List<Customer> Customers { get; set; }
}

// Nếu có thêm danh sách khác như sản phẩm:
public class ProductListViewModel : PaginationViewModel
{
    public List<Product> Products { get; set; }
}
