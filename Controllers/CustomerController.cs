using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System;
using MVC.Models;  // Đảm bảo thêm đúng namespace cho Pet model

namespace YourNamespace.Controllers
{
    public class CustomerController : Controller
    {
        private readonly string _connectionString = Environment.GetEnvironmentVariable("ORACLE_CONN_STRING");

        [HttpGet]
        public IActionResult GetCustomers()
        {
            List<Customer> Customers = new List<Customer>();

            using (var conn = new OracleConnection(_connectionString))
            {
                try
                {
                    conn.Open();
                    using (var cmd = new OracleCommand("SELECT customer_id, first_name, last_name, phone_number, email, address FROM usertest.customer", conn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Customers.Add(new Customer
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
                catch (Exception ex)
                {
                    return StatusCode(500, "Internal server error: " + ex.Message);
                }
            }

            return Ok(Customers);
        }
        // Hiển thị danh sách khách hàng
        [HttpGet("/Khachhang")]
        public IActionResult Index(int page = 1, int pageSize = 5)
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



        [HttpGet("/Customer/LoadCustomerPartial")]
        public IActionResult LoadCustomerPartial(string keyword = "", int page = 1, int pageSize = 5)
        {
            List<Customer> filteredCustomer = new();

            try
            {
                using (var conn = new OracleConnection(_connectionString))
                {
                    conn.Open();

                    keyword = keyword?.ToLower() ?? "";

                    string query = @"
                SELECT customer_id, first_name, last_name, phone_number, email, address
                FROM usertest.customer
                WHERE LOWER(last_name) LIKE :keyword
                ORDER BY customer_id ASC";

                    using (var cmd = new OracleCommand(query, conn))
                    {
                        cmd.Parameters.Add("keyword", OracleDbType.Varchar2).Value = $"%{keyword}%";

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
                Console.WriteLine("Lỗi LoadCustomerPartial: " + ex.Message);
                return StatusCode(500, "Lỗi server: " + ex.Message);
            }

            int totalItems = filteredCustomer.Count;
            int countPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            var customersOnPage = filteredCustomer.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var model = new CustomerListViewModel
            {
                Customers = customersOnPage,
                CurrentPage = page,
                CountPages = countPages,
                GenerateUrl = p => Url.Action("LoadCustomerPartial", new { keyword = keyword, page = p })!
            };

            return PartialView("~/Views/Customer/_CustomerListPartial.cshtml", model);
        }


        //Hiển thị form tạo mới khách hàng
        [HttpGet("Customer/CreateCustomers")]
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
                            TempData["Success"] = " Them khach hang thanh cong!";
                            return RedirectToAction("Index");
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

        // xóa khách hàng
        [HttpPost]
        public IActionResult DeleteCustomer(int id, int page = 1)
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
                            TempData["Success"] = "Xoa khach hang thanh cong!";
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

            return RedirectToAction("Index", new { page = page });

        }

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
        public IActionResult EditCustomer(Customer customer, int page = 1)
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
                            TempData["Success"] = " Cap nhat khach hang thanh cong!";
                            return RedirectToAction("Index", new { page = page });
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
    }
}
