using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Collections.Generic;
using System;
using MVC.Models;  // Đảm bảo thêm đúng namespace cho Pet model

namespace YourNamespace.Controllers
{

    public class OrderController : Controller
    {
        private readonly string _connectionString = Environment.GetEnvironmentVariable("ORACLE_CONN_STRING");

        [HttpGet]
        public IActionResult GetOrders()
        {
            List<Order> orders = new List<Order>();

            using (var conn = new OracleConnection(_connectionString))
            {
                try
                {
                    conn.Open();
                    using (var cmd = new OracleCommand("SELECT a.order_id, a.customer_id, a.order_date, a.total_amount FROM orders a;", conn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                orders.Add(new Order
                                {
                                    OrderId = reader.GetInt32(0),
                                    CustomerId = reader.GetInt32(1),
                                    OrderDate = reader.GetDateTime(2),
                                    TotalAmount = reader.GetDecimal(3)
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

            return Ok(orders);
        }
        [HttpGet("/Hoadon")]
        public IActionResult Index(int page = 1, int pageSize = 5)
        {
            List<Order> allOrders = new List<Order>();

            try
            {
                using (var conn = new OracleConnection(_connectionString))
                {
                    conn.Open();
                    string query = "SELECT order_id, customer_id, order_date, total_amount FROM orders order by order_id asc";

                    using (var cmd = new OracleCommand(query, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            allOrders.Add(new Order
                            {
                                OrderId = reader.GetInt32(0),
                                CustomerId = reader.GetInt32(1),
                                OrderDate = reader.GetDateTime(2),
                                TotalAmount = reader.GetDecimal(3)
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
            int totalItems = allOrders.Count;
            int countPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var ordersOnPage = allOrders
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var model = new OrderListViewModel
            {
                Orders = ordersOnPage,
                CurrentPage = page,
                CountPages = countPages,
                GenerateUrl = p => Url.Action("Hoadon", new { page = p })!
            };

            return View(model);
        }

        [HttpGet("/Order/LoadOrdersPartial")]
        public IActionResult LoadOrdersPartial(string keyword = "", int page = 1, int pageSize = 5)
        {
            List<Order> filteredOrders = new List<Order>();

            try
            {
                using (var conn = new OracleConnection(_connectionString))
                {
                    conn.Open();
                    string query = "SELECT a.order_id, a.customer_id, a.order_date, a.total_amount FROM orders a WHERE TO_CHAR(a.order_date, 'YYYY-MM-DD') LIKE :keyword ORDER BY order_id ASC";

                    using (var cmd = new OracleCommand(query, conn))
                    {
                        cmd.Parameters.Add(new OracleParameter("keyword", $"%{keyword.ToLower()}%"));

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                filteredOrders.Add(new Order
                                {
                                    OrderId = reader.GetInt32(0),
                                    CustomerId = reader.GetInt32(1),
                                    OrderDate = reader.GetDateTime(2),
                                    TotalAmount = reader.GetDecimal(3)
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

            int totalItems = filteredOrders.Count;
            int countPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            var ordersOnPage = filteredOrders.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var model = new OrderListViewModel
            {
                Orders = ordersOnPage,
                CurrentPage = page,
                CountPages = countPages,
                GenerateUrl = p => Url.Action("LoadOrdersPartial", new { keyword = keyword, page = p })!
            };

            return PartialView("~/Views/Order/_OrderListPartial.cshtml", model);
        }

        [HttpGet("Order/CreateOrders")]
        public IActionResult CreateOrders()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateOrders(Order order)
        {
            if (!ModelState.IsValid)
            {
                return View(order);
            }

            using (var conn = new OracleConnection(_connectionString))
            {
                try
                {
                    conn.Open();

                    using (var cmd = new OracleCommand("add_order", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("p_order_id", OracleDbType.Int32).Value = order.OrderId;
                        cmd.Parameters.Add("p_customer_id", OracleDbType.Int32).Value = order.CustomerId;
                        cmd.Parameters.Add("p_order_date", OracleDbType.Date).Value = order.OrderDate;
                        cmd.Parameters.Add("p_total_amount", OracleDbType.Decimal).Value = order.TotalAmount;

                        // Tham số OUT để nhận message
                        var messageParam = new OracleParameter("p_message", OracleDbType.Varchar2, 4000)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(messageParam);

                        cmd.ExecuteNonQuery();

                        string resultMessage = messageParam.Value.ToString();

                        if (resultMessage.Contains("Them hoa don thanh cong!"))
                        {
                            TempData["Success"] = resultMessage;
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            ModelState.AddModelError("", resultMessage);
                        }
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Lỗi: " + ex.Message);
                }
            }

            return View(order);
        }


        [HttpPost]
        public IActionResult DeleteOrder(int id, int page = 1)
        {
            try
            {
                using (var conn = new OracleConnection(_connectionString))
                {
                    conn.Open();

                    using (var cmd = new OracleCommand("delete_order", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Tham số input
                        cmd.Parameters.Add("p_order_id", OracleDbType.Int32).Value = id;

                        // Tham số output để lấy message
                        cmd.Parameters.Add("p_message", OracleDbType.Varchar2, 200).Direction = ParameterDirection.Output;

                        cmd.ExecuteNonQuery();

                        string message = cmd.Parameters["p_message"].Value.ToString();

                        if (message.Contains("Xoa don hang thanh cong!"))
                        {
                            TempData["Success"] = message;
                            return RedirectToAction("Index", new { page = page });
                        }
                        else
                        {
                            TempData["Error"] = message;
                        }
                    }

                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Lỗi khi gọi procedure: " + ex.Message;
            }

            return RedirectToAction("Index", new { page = page });
        }

        [HttpGet]
        public IActionResult EditOrder(int id)
        {
            Order order = null;

            try
            {
                using (var conn = new OracleConnection(_connectionString))
                {
                    conn.Open();

                    string selectQuery = "SELECT a.order_id, a.customer_id, a.order_date, a.total_amount FROM orders a WHERE order_id = :OrderId";

                    using (var cmd = new OracleCommand(selectQuery, conn))
                    {
                        cmd.Parameters.Add(new OracleParameter("OrderId", id));

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                order = new Order
                                {
                                    OrderId = reader.GetInt32(0),
                                    CustomerId = reader.GetInt32(1),
                                    OrderDate = reader.GetDateTime(2),
                                    TotalAmount = reader.GetDecimal(3)
                                };
                            }
                        }
                    }
                }

                if (order == null)
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi lấy thú cưng: " + ex.Message);
                return View("Error");
            }

            return View(order);
        }

        // post thông tin đã edit 
[HttpPost]
public IActionResult EditOrder(Order order, int page = 1)
{
    if (!ModelState.IsValid)
        return View(order);

    try
    {
        using (var conn = new OracleConnection(_connectionString))
        {
            conn.Open();

            // Gọi stored procedure
            using (var cmd = new OracleCommand("update_order", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                // Thêm các tham số đầu vào
                cmd.Parameters.Add("p_order_id", OracleDbType.Int32).Value = order.OrderId;
                cmd.Parameters.Add("p_customer_id", OracleDbType.Int32).Value = order.CustomerId;
                cmd.Parameters.Add("p_order_date", OracleDbType.Date).Value = order.OrderDate;
                cmd.Parameters.Add("p_total_amount", OracleDbType.Decimal).Value = order.TotalAmount;

                // Thêm tham số OUT để nhận thông điệp
                var messageParam = new OracleParameter("p_message", OracleDbType.Varchar2, 4000)
                    {
                        Direction = ParameterDirection.Output
                    };
                cmd.Parameters.Add(messageParam);

                // Thực thi stored procedure
                cmd.ExecuteNonQuery();

                // Lấy thông điệp trả về
                string resultMessage = cmd.Parameters["p_message"].Value.ToString();

                // Kiểm tra kết quả trả về
                if (resultMessage.Contains("Cap nhat don hang thanh cong!"))
                {
                    TempData["Success"] = resultMessage;
                    return RedirectToAction("Index", new { page = page });
                }
                else
                {
                    ModelState.AddModelError("", resultMessage);
                }
            }
        }
    }
    catch (Exception ex)
    {
        ModelState.AddModelError("", "Lỗi khi cập nhật: " + ex.Message);
    }

    return View(order);
}


    }
}
