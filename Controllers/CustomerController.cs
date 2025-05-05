using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using System.Data;
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

                    using (var cmd = new OracleCommand("add_customer", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Thêm các tham số đầu vào cho thủ tục
                        cmd.Parameters.Add(new OracleParameter("p_customer_id", customer.CustomerId));
                        cmd.Parameters.Add(new OracleParameter("p_first_name", customer.FirstName));
                        cmd.Parameters.Add(new OracleParameter("p_last_name", customer.LastName));
                        cmd.Parameters.Add(new OracleParameter("p_phone_number", customer.PhoneNumber));
                        cmd.Parameters.Add(new OracleParameter("p_email", customer.Email));
                        cmd.Parameters.Add(new OracleParameter("p_address", customer.Address));

                        // Thêm tham số OUT để nhận thông báo
                        var messageParam = new OracleParameter("p_message", OracleDbType.Varchar2, 4000)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(messageParam);

                        // Thực thi thủ tục
                        cmd.ExecuteNonQuery();

                        // Lấy thông báo từ thủ tục và hiển thị
                        string message = messageParam.Value.ToString();

                        if (message.Contains("Them khach hang thanh cong!"))
                        {
                            TempData["Success"] = message;
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            ModelState.AddModelError("", message);
                        }

                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Lỗi: " + ex.Message);
                }
            }

            // Trả về lại view nếu có lỗi
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

                    // Gọi stored procedure để xóa khách hàng
                    string deleteCustomerProcedure = "delete_customer";  // Tên stored procedure đã tạo
                    using (var cmd = new OracleCommand(deleteCustomerProcedure, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Thêm tham số vào stored procedure
                        cmd.Parameters.Add(new OracleParameter("p_customer_id", id));

                        // Thêm tham số OUT để nhận thông báo
                        var messageParam = new OracleParameter("p_message", OracleDbType.Varchar2, 4000)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(messageParam);

                        // Thực thi stored procedure
                        cmd.ExecuteNonQuery();

                        // Lấy thông báo trả về từ stored procedure
                        string resultMessage = messageParam.Value.ToString();

                        if (resultMessage.Contains("Xoa khach hang thanh cong!"))
                        {
                            TempData["Success"] = resultMessage;
                            return RedirectToAction("Index", new { page = page });
                        }
                        else
                        {
                            ModelState.AddModelError("", resultMessage);
                        }
                    }

                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Lỗi khi xóa: " + ex.Message;
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

                    using (var cmd = new OracleCommand("update_customer", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("p_customer_id", OracleDbType.Int32).Value = customer.CustomerId;
                        cmd.Parameters.Add("p_first_name", OracleDbType.Varchar2).Value = customer.FirstName;
                        cmd.Parameters.Add("p_last_name", OracleDbType.Varchar2).Value = customer.LastName;
                        cmd.Parameters.Add("p_phone_number", OracleDbType.Varchar2).Value = customer.PhoneNumber;
                        cmd.Parameters.Add("p_email", OracleDbType.Varchar2).Value = customer.Email;
                        cmd.Parameters.Add("p_address", OracleDbType.Varchar2).Value = customer.Address;

                        // Output parameter
                        var messageParam = new OracleParameter("p_message", OracleDbType.Varchar2, 4000);
                        messageParam.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(messageParam);

                        cmd.ExecuteNonQuery();

                        string message = messageParam.Value?.ToString();

                        if (message.Contains("Cap nhat thong tin khach hang thanh cong!"))
                        {
                            TempData["Success"] = message;
                            return RedirectToAction("Index", new { page = page });
                        }
                        else
                        {
                            ModelState.AddModelError("", message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Lỗi hệ thống: " + ex.Message);
            }

            return View(customer);
        }

    }
}
