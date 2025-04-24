using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using MVC.Models;
using System;
using System.Collections.Generic;

public class HomeController : Controller
{
    private readonly string _connectionString = Environment.GetEnvironmentVariable("ORACLE_CONN_STRING");


    // Hiển thị danh sách thú cưng
    [HttpGet]
    public IActionResult Thucung(int page = 1, int pageSize = 5)
    {
        List<Pet> allPets = new List<Pet>();

        try
        {
            using (var conn = new OracleConnection(_connectionString))
            {
                conn.Open();
                string query = "SELECT pet_id, pet_name, pet_type, breed, age, gender, price, stock FROM usertest.pet order by pet_id asc";

                using (var cmd = new OracleCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        allPets.Add(new Pet
                        {
                            PetId = reader.GetInt32(0),
                            PetName = reader.GetString(1),
                            PetType = reader.GetString(2),
                            Breed = reader.GetString(3),
                            Age = reader.GetInt32(4),
                            Gender = reader.GetString(5),
                            Price = reader.GetDecimal(6),
                            Stock = reader.GetInt32(7)
                        });
                    }
                }
            }
        }
        catch (OracleException ex)
        {
            // Handle error here
        }

        // Phân trang
        int totalItems = allPets.Count;
        int countPages = (int)Math.Ceiling(totalItems / (double)pageSize);

        var petsOnPage = allPets
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var model = new PetListViewModel
        {
            Pets = petsOnPage,
            CurrentPage = page,
            CountPages = countPages,
            GenerateUrl = p => Url.Action("Thucung", new { page = p })!
        };

        return View(model);
    }

    [HttpGet]
    public IActionResult LoadPetsPartial(string keyword = "", int page = 1, int pageSize = 5)
    {
        List<Pet> filteredPets = new List<Pet>();

        try
        {
            using (var conn = new OracleConnection(_connectionString))
            {
                conn.Open();
                string query = "SELECT pet_id, pet_name, pet_type, breed, age, gender, price, stock FROM usertest.pet WHERE LOWER(pet_name) LIKE :keyword ORDER BY pet_id ASC";

                using (var cmd = new OracleCommand(query, conn))
                {
                    cmd.Parameters.Add(new OracleParameter("keyword", $"%{keyword.ToLower()}%"));

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            filteredPets.Add(new Pet
                            {
                                PetId = reader.GetInt32(0),
                                PetName = reader.GetString(1),
                                PetType = reader.GetString(2),
                                Breed = reader.GetString(3),
                                Age = reader.GetInt32(4),
                                Gender = reader.GetString(5),
                                Price = reader.GetDecimal(6),
                                Stock = reader.GetInt32(7)
                            });
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            return Content("Lỗi: " + ex.Message);
        }

        int totalItems = filteredPets.Count;
        int countPages = (int)Math.Ceiling(totalItems / (double)pageSize);
        var petsOnPage = filteredPets.Skip((page - 1) * pageSize).Take(pageSize).ToList();

        var model = new PetListViewModel
        {
            Pets = petsOnPage,
            CurrentPage = page,
            CountPages = countPages,
            GenerateUrl = p => Url.Action("LoadPetsPartial", new { keyword = keyword, page = p })!
        };

        return PartialView("_PetListPartial", model);
    }



    // ----------------------------------------------------------------------------------------------------------------------

    // Hiển thị danh sách khách hàng
    [HttpGet]
    public IActionResult Khachhang(int page = 1, int pageSize = 5)
    {
        List<Customer> allCustomers = new List<Customer>();

        try
        {
            using (var conn = new OracleConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new OracleCommand("SELECT customer_id, first_name, last_name, phone_number, email, address FROM usertest.customer order by customer_id asc", conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        allCustomers.Add(new Customer
                        {
                            CustomerId = reader.GetInt32(0),
                            FirstName = reader.GetString(1),
                            LastName = reader.GetString(2),
                            PhoneNumber = reader.GetString(3),
                            Email = reader.GetString(4),
                            Address = reader.GetString(5),
                        });
                    }
                }
            }
        }
        catch (OracleException ex)
        {
        }
        // phan trang
        int totalItems = allCustomers.Count;
        int countPages = (int)Math.Ceiling(totalItems / (double)pageSize);

        var customersOnPage = allCustomers
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var model = new CustomerListViewModel
        {
            Customers = customersOnPage,
            CurrentPage = page,
            CountPages = countPages,
            GenerateUrl = p => Url.Action("Khachhang", new { page = p })!
        };

        return View(model);

    }



    [HttpGet]
    public IActionResult LoadCustomerPartial(string keyword = "", int page = 1, int pageSize = 5)
    {
        List<Customer> filteredCustomer = new List<Customer>();

        try
        {
            using (var conn = new OracleConnection(_connectionString))
            {
                conn.Open();
                string query = "SELECT customer_id, first_name, last_name, phone_number, email, address FROM usertest.customer WHERE LOWER(last_name) LIKE :keyword ORDER BY customer_id ASC";

                using (var cmd = new OracleCommand(query, conn))
                {
                    cmd.Parameters.Add(new OracleParameter("keyword", $"%{keyword.ToLower()}%"));

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            filteredCustomer.Add(new Customer
                            {
                            CustomerId = reader.GetInt32(0),
                            FirstName = reader.GetString(1),
                            LastName = reader.GetString(2),
                            PhoneNumber = reader.GetString(3),
                            Email = reader.GetString(4),
                            Address = reader.GetString(5),
                            });
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            return Content("Lỗi: " + ex.Message);
        }

        int totalItems = filteredCustomer.Count;
        int countPages = (int)Math.Ceiling(totalItems / (double)pageSize);
        var customersOnPage = filteredCustomer.Skip((page - 1) * pageSize).Take(pageSize).ToList();

        var model = new CustomerListViewModel
        {
            Customers = customersOnPage,
            CurrentPage = page,
            CountPages = countPages,
            GenerateUrl = p => Url.Action("LoadCustomersPartial", new { keyword = keyword, page = p })!
        };

        return PartialView("_CustomerListPartial", model);
    }
    // ----------------------------------------------------------------------------------------------------------------------

    //Hiển thị form tạo mới thú cưng
    [HttpGet]
    public IActionResult CreatePets()
    {
        return View();
    }
    // Phương thức post gửi thông tin từ view để tạo mới thú cưng
    [HttpPost]
    public IActionResult CreatePets(Pet pet)
    {
        if (!ModelState.IsValid)
        {
            return View(pet);
        }

        using (var conn = new OracleConnection(_connectionString))
        {
            try
            {
                conn.Open();

                string insertQuery = @"
                    INSERT INTO usertest.pet (pet_id, pet_name, pet_type, breed, age, gender, price, stock)
                    VALUES (:PetId, :PetName, :PetType, :Breed, :Age, :Gender, :Price, :Stock)";

                using (var cmd = new OracleCommand(insertQuery, conn))
                {
                    cmd.Parameters.Add(new OracleParameter("PetId", pet.PetId));
                    cmd.Parameters.Add(new OracleParameter("PetName", pet.PetName));
                    cmd.Parameters.Add(new OracleParameter("PetType", pet.PetType));
                    cmd.Parameters.Add(new OracleParameter("Breed", pet.Breed));
                    cmd.Parameters.Add(new OracleParameter("Age", pet.Age));
                    cmd.Parameters.Add(new OracleParameter("Gender", pet.Gender));
                    cmd.Parameters.Add(new OracleParameter("Price", pet.Price));
                    cmd.Parameters.Add(new OracleParameter("Stock", pet.Stock));

                    int result = cmd.ExecuteNonQuery();
                    if (result > 0)
                    {
                        TempData["Success"] = " Thêm thú cưng thành công!";
                        return RedirectToAction("Thucung");
                    }
                    else
                    {
                        ModelState.AddModelError("", "   Thêm thất bại.");
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Lỗi: " + ex.Message);
            }
        }
        return View(pet);
    }

    // ------------------------------------------------------------------------------------------------------------

    //Hiển thị form tạo mới khách hàng
    [HttpGet]
    public IActionResult CreateCustomers()
    {
        return View();
    }
    // Phương thức post gửi thông tin từ view để tạo mới khách hàng
    [HttpPost]
    public IActionResult CreateCustomers(Customer customer)
    {
        if (!ModelState.IsValid)
        {
            return View(customer);
        }

        using (var conn = new OracleConnection(_connectionString))
        {
            try
            {
                conn.Open();

                string insertQuery = @"
                    INSERT INTO usertest.customer (customer_id, first_name, last_name, phone_number, email, address)
                    VALUES (:CustomerId, :FirstName, :LastName, :PhoneNumber, :Email, :Address)";

                using (var cmd = new OracleCommand(insertQuery, conn))
                {
                    cmd.Parameters.Add(new OracleParameter("CustomerId", customer.CustomerId));
                    cmd.Parameters.Add(new OracleParameter("FirstName", customer.FirstName));
                    cmd.Parameters.Add(new OracleParameter("LastName", customer.LastName));
                    cmd.Parameters.Add(new OracleParameter("PhoneNumber", customer.PhoneNumber));
                    cmd.Parameters.Add(new OracleParameter("Email", customer.Email));
                    cmd.Parameters.Add(new OracleParameter("Address", customer.Address));

                    int result = cmd.ExecuteNonQuery();
                    if (result > 0)
                    {
                        TempData["Success"] = " Thêm Khách hàng thành công!";
                        return RedirectToAction("Khachhang");
                    }
                    else
                    {
                        ModelState.AddModelError("", "   Thêm thất bại.");
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Lỗi: " + ex.Message);
            }
        }
        return View(customer);
    }
    // ---------------------------------------------------------------------------------------------------

    // dùng post để xóa thú cưng
    [HttpPost]
    public IActionResult DeletePet(int id)
    {
        try
        {
            using (var conn = new OracleConnection(_connectionString))
            {
                conn.Open();
                string deleteQuery = "DELETE FROM usertest.pet WHERE pet_id = :PetId";
                using (var cmd = new OracleCommand(deleteQuery, conn))
                {
                    cmd.Parameters.Add(new OracleParameter("PetId", id));
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        TempData["Success"] = " Xóa thú cưng thành công!";
                    }
                    else
                    {
                        TempData["Error"] = "   Không tìm thấy thú cưng để xóa.";
                    }
                }

                conn.Close();
            }
        }
        catch (Exception ex)
        {
            TempData["Error"] = "   Lỗi khi xóa: " + ex.Message;
        }

        return RedirectToAction("Thucung");

    }

    // -------------------------------------------------------------------------------------------------------------------------

    // xóa khách hàng
    [HttpPost]
    public IActionResult DeleteCustomer(int id)
    {
        try
        {
            using (var conn = new OracleConnection(_connectionString))
            {
                conn.Open();
                string deleteQuery = "DELETE FROM usertest.customer WHERE customer_id = :CustomerId";
                using (var cmd = new OracleCommand(deleteQuery, conn))
                {
                    cmd.Parameters.Add(new OracleParameter("CustomerId", id));
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        TempData["Success"] = " Xóa khách hàng thành công!";
                    }
                    else
                    {
                        TempData["Error"] = "   Không tìm thấy khách hàng để xóa.";
                    }
                }

                conn.Close();
            }
        }
        catch (Exception ex)
        {
            TempData["Error"] = "   Lỗi khi xóa: " + ex.Message;
        }

        return RedirectToAction("Khachhang");

    }
    // ---------------------------------------------------------------------------------------------------------------

    // Get thông tin thú cưng ra trang edit
    [HttpGet]
    public IActionResult EditPet(int id)
    {
        Pet pet = null;

        try
        {
            using (var conn = new OracleConnection(_connectionString))
            {
                conn.Open();

                string selectQuery = "SELECT pet_id, pet_name, pet_type, breed, age, gender, price, stock FROM usertest.pet WHERE pet_id = :PetId";

                using (var cmd = new OracleCommand(selectQuery, conn))
                {
                    cmd.Parameters.Add(new OracleParameter("PetId", id));

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            pet = new Pet
                            {
                                PetId = reader.GetInt32(0),
                                PetName = reader.GetString(1),
                                PetType = reader.GetString(2),
                                Breed = reader.GetString(3),
                                Age = reader.GetInt32(4),
                                Gender = reader.GetString(5),
                                Price = reader.GetDecimal(6),
                                Stock = reader.GetInt32(7)
                            };
                        }
                    }
                }
            }

            if (pet == null)
            {
                return NotFound();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Lỗi khi lấy thú cưng: " + ex.Message);
            return View("Error");
        }

        return View(pet);
    }

    // post thông tin đã edit 
    [HttpPost]
    public IActionResult EditPet(Pet pet)
    {
        if (!ModelState.IsValid)
            return View(pet);

        try
        {
            using (var conn = new OracleConnection(_connectionString))
            {
                conn.Open();

                string updateQuery = @"
                UPDATE usertest.pet 
                SET pet_name = :PetName, pet_type = :PetType, breed = :Breed, 
                    age = :Age, gender = :Gender, price = :Price, stock = :Stock
                WHERE pet_id = :PetId";

                using (var cmd = new OracleCommand(updateQuery, conn))
                {
                    cmd.Parameters.Add(new OracleParameter("PetName", pet.PetName));
                    cmd.Parameters.Add(new OracleParameter("PetType", pet.PetType));
                    cmd.Parameters.Add(new OracleParameter("Breed", pet.Breed));
                    cmd.Parameters.Add(new OracleParameter("Age", pet.Age));
                    cmd.Parameters.Add(new OracleParameter("Gender", pet.Gender));
                    cmd.Parameters.Add(new OracleParameter("Price", pet.Price));
                    cmd.Parameters.Add(new OracleParameter("Stock", pet.Stock));
                    cmd.Parameters.Add(new OracleParameter("PetId", pet.PetId));

                    int result = cmd.ExecuteNonQuery();

                    if (result > 0)
                    {
                        TempData["Success"] = " Cập nhật thành công!";
                        return RedirectToAction("Thucung");
                    }
                    else
                    {
                        ModelState.AddModelError("", "   Không tìm thấy thú cưng.");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", "   Lỗi cập nhật: " + ex.Message);
        }

        return View(pet);
    }
    // -----------------------------------------------------------------------------------------------------------------



    [HttpGet]
    public IActionResult EditCustomer(int id)
    {
        Customer customer = null;

        try
        {
            using (var conn = new OracleConnection(_connectionString))
            {
                conn.Open();

                string selectQuery = "SELECT customer_id, first_name, last_name, phone_number, email, address FROM usertest.customer WHERE customer_id = :CustomerId";

                using (var cmd = new OracleCommand(selectQuery, conn))
                {
                    cmd.Parameters.Add(new OracleParameter("CustomerId", id));

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            customer = new Customer
                            {
                                CustomerId = reader.GetInt32(0),
                                FirstName = reader.GetString(1),
                                LastName = reader.GetString(2),
                                PhoneNumber = reader.GetString(3),
                                Email = reader.GetString(4),
                                Address = reader.GetString(5),
                            };
                        }
                    }
                }
            }

            if (customer == null)
            {
                return NotFound();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Lỗi khi lấy khách hàng: " + ex.Message);
            return View("Error");
        }

        return View(customer);
    }


    [HttpPost]
    public IActionResult EditCustomer(Customer customer)
    {
        if (!ModelState.IsValid)
            return View(customer);

        try
        {
            using (var conn = new OracleConnection(_connectionString))
            {
                conn.Open();

                string updateQuery = @"
                UPDATE usertest.customer 
                SET first_name = :FirstName, last_name = :LastName, phone_number = :PhoneNumber, 
                    email = :Email, address = :Address
                WHERE customer_id = :CustomerId";

                using (var cmd = new OracleCommand(updateQuery, conn))
                {
                    cmd.Parameters.Add(new OracleParameter("FirstName", customer.FirstName));
                    cmd.Parameters.Add(new OracleParameter("LastName", customer.LastName));
                    cmd.Parameters.Add(new OracleParameter("PhoneNumber", customer.PhoneNumber));
                    cmd.Parameters.Add(new OracleParameter("Email", customer.Email));
                    cmd.Parameters.Add(new OracleParameter("Address", customer.Address));
                    cmd.Parameters.Add(new OracleParameter("CustomerId", customer.CustomerId));

                    int result = cmd.ExecuteNonQuery();

                    if (result > 0)
                    {
                        TempData["Success"] = " Cập nhật thành công!";
                        return RedirectToAction("Khachhang");
                    }
                    else
                    {
                        ModelState.AddModelError("", "   Không tìm thấy khách hàng.");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", "   Lỗi cập nhật: " + ex.Message);
        }

        return View(customer);
    }

    // ----------------------------------------------------------------------------------------------------------------




    [HttpGet]
    public IActionResult Sanpham(int page = 1, int pageSize= 5)
    {
        List<Product> allProducts = new List<Product>();

        try
        {
            using (var conn = new OracleConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new OracleCommand("SELECT product_id, product_name, category, price, stock FROM usertest.product order by product_id asc", conn)) 
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        allProducts.Add(new Product
                        {
                            ProductId = reader.GetInt32(0),
                            ProductName = reader.GetString(1),
                            ProductCategory = reader.GetString(2),
                            ProductPrice = reader.GetDecimal(3),
                            ProductStock = reader.GetInt32(4),
                        });
                    }
                }
            }
        }
        catch (OracleException ex)
        {
            // Log lỗi nếu cần
            ViewBag.ErrorMessage = "Lỗi kết nối CSDL: " + ex.Message;
            return View("Error");
        }
        // Phân trang
        int totalItems = allProducts.Count;
        int countPages = (int)Math.Ceiling(totalItems / (double)pageSize);

        var productsOnPage = allProducts
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var model = new ProductListViewModel
        {
            Products = productsOnPage,
            CurrentPage = page,
            CountPages = countPages,
            GenerateUrl = p => Url.Action("Sanpham", new { page = p })!
        };

        return View(model);
    }

        [HttpGet]
    public IActionResult LoadProductsPartial(string keyword = "", int page = 1, int pageSize = 5)
    {
        List<Product> filteredProducts = new List<Product>();

        try
        {
            using (var conn = new OracleConnection(_connectionString))
            {
                conn.Open();
                string query = "SELECT product_id, product_name, category, price, stock FROM usertest.product WHERE LOWER(product_name) LIKE :keyword ORDER BY product_id ASC";

                using (var cmd = new OracleCommand(query, conn))
                {
                    cmd.Parameters.Add(new OracleParameter("keyword", $"%{keyword.ToLower()}%"));

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            filteredProducts.Add(new Product
                            {
                            ProductId = reader.GetInt32(0),
                            ProductName = reader.GetString(1),
                            ProductCategory = reader.GetString(2),
                            ProductPrice = reader.GetDecimal(3),
                            ProductStock = reader.GetInt32(4),
                            });
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            return Content("Lỗi: " + ex.Message);
        }

        int totalItems = filteredProducts.Count;
        int countPages = (int)Math.Ceiling(totalItems / (double)pageSize);
        var productsOnPage = filteredProducts.Skip((page - 1) * pageSize).Take(pageSize).ToList();

        var model = new ProductListViewModel
        {
            Products = productsOnPage,
            CurrentPage = page,
            CountPages = countPages,
            GenerateUrl = p => Url.Action("LoadProductsPartial", new { keyword = keyword, page = p })!
        };

        return PartialView("_ProductListPartial", model);
    } 




    // -------------------------------------------------------------------------------------------------------------------------





    [HttpGet]
    public IActionResult CreateProducts()
    {
        return View();
    }

    [HttpPost]
    public IActionResult CreateProducts(Product product)
    {
        if (!ModelState.IsValid)
        {
            return View(product);
        }

        using (var conn = new OracleConnection(_connectionString))
        {
            try
            {
                conn.Open();

                string insertQuery = @"
                    INSERT INTO usertest.product (product_id, product_name, category, price, stock)
                    VALUES (:ProductId, :ProductName, :ProductCategory, :ProductPrice, :ProductStock)";

                using (var cmd = new OracleCommand(insertQuery, conn))
                {
                    cmd.Parameters.Add(new OracleParameter("ProductId", product.ProductId));
                    cmd.Parameters.Add(new OracleParameter("ProductName", product.ProductName));
                    cmd.Parameters.Add(new OracleParameter("ProductCategory", product.ProductCategory));
                    cmd.Parameters.Add(new OracleParameter("ProductPrice", product.ProductPrice));
                    cmd.Parameters.Add(new OracleParameter("ProductStock", product.ProductStock));
                    int result = cmd.ExecuteNonQuery();
                    if (result > 0)
                    {
                        TempData["Success"] = " Thêm Sản phẩm thành công!";
                        return RedirectToAction("Sanpham");
                    }
                    else
                    {
                        ModelState.AddModelError("", "   Thêm thất bại.");
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Lỗi: " + ex.Message);
            }
        }
        return View(product);
    }


    // -----------------------------------------------------------------------------------------------------------------------------



    [HttpGet]
    public IActionResult EditProduct(int id)
    {
        Product product = null;

        try
        {
            using (var conn = new OracleConnection(_connectionString))
            {
                conn.Open();

                string selectQuery = "SELECT product_id, product_name, category, price, stock FROM usertest.product WHERE product_id = :ProductId";

                using (var cmd = new OracleCommand(selectQuery, conn))
                {
                    cmd.Parameters.Add(new OracleParameter("ProductId", id));

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            product = new Product
                            {
                                ProductId = reader.GetInt32(0),
                                ProductName = reader.GetString(1),
                                ProductCategory = reader.GetString(2),
                                ProductPrice = reader.GetDecimal(3),
                                ProductStock = reader.GetInt32(4),
                            };
                        }
                    }
                }
            }

            if (product == null)
            {
                return NotFound();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Lỗi khi lấy thông tin sản phẩm/dịch vụ: " + ex.Message);
            return View("Error");
        }

        return View(product);
    }


    [HttpPost]
    public IActionResult EditProduct(Product product)
    {
        if (!ModelState.IsValid)
            return View(product);

        try
        {
            using (var conn = new OracleConnection(_connectionString))
            {
                conn.Open();

                string updateQuery = @"
                UPDATE usertest.product
                SET product_name = :ProductName,
                    category = :ProductCategory,
                    price = :ProductPrice,
                    stock = :ProductStock
                WHERE product_id = :ProductId";

                using (var cmd = new OracleCommand(updateQuery, conn))
                {
                    cmd.Parameters.Add(new OracleParameter("ProductName", product.ProductName));
                    cmd.Parameters.Add(new OracleParameter("ProductCategory", product.ProductCategory));
                    cmd.Parameters.Add(new OracleParameter("ProductPrice", product.ProductPrice));
                    cmd.Parameters.Add(new OracleParameter("ProductStock", product.ProductStock));
                    cmd.Parameters.Add(new OracleParameter("ProductId", product.ProductId));

                    int result = cmd.ExecuteNonQuery();

                    if (result > 0)
                    {
                        TempData["Success"] = " Cập nhật thành công!";
                        return RedirectToAction("Sanpham");
                    }
                    else
                    {
                        ModelState.AddModelError("", "   Không tìm thấy sản phẩm cần cập nhật.");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", "   Lỗi cập nhật: " + ex.Message);
        }

        return View(product);
    }


    // --------------------------------------------------------------------------------------------------------------------------------


    [HttpPost]
    public IActionResult DeleteProduct(int id)
    {
        try
        {
            using (var conn = new OracleConnection(_connectionString))
            {
                conn.Open();
                string deleteQuery = "DELETE FROM usertest.product WHERE product_id = :ProductId";
                using (var cmd = new OracleCommand(deleteQuery, conn))
                {
                    cmd.Parameters.Add(new OracleParameter("ProductId", id));
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        TempData["Success"] = " Xóa sản phẩm thành công!";
                    }
                    else
                    {
                        TempData["Error"] = "   Không tìm thấy sản phẩm để xóa.";
                    }
                }

                conn.Close();
            }
        }
        catch (Exception ex)
        {
            TempData["Error"] = "   Lỗi khi xóa: " + ex.Message;
        }

        return RedirectToAction("Sanpham");

    }



    // ----------------------------------------------------------------------------------------------------------------------------------



    public IActionResult Intro()
    {
        return View();
    }
    public IActionResult Hoadon()
    {
        return View();
    }
    public IActionResult Hoadonchitiet()
    {
        return View();
    }
}

