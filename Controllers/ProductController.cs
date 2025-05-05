using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Collections.Generic;
using System;
using MVC.Models;  // Đảm bảo thêm đúng namespace cho Pet model

namespace YourNamespace.Controllers
{
    public class ProductController : Controller
    {
        private readonly string _connectionString = Environment.GetEnvironmentVariable("ORACLE_CONN_STRING");

        [HttpGet]
        public IActionResult GetProduct()
        {
            List<Product> products = new List<Product>();

            using (var conn = new OracleConnection(_connectionString))
            {
                try
                {
                    conn.Open();
                    using (var cmd = new OracleCommand("SELECT product_id, product_name, category, price, stock FROM usertest.product;", conn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                products.Add(new Product
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
                catch (Exception ex)
                {
                    return StatusCode(500, "Internal server error: " + ex.Message);
                }
            }

            return Ok(products);
        }
        [HttpGet("/Sanpham")]
        public IActionResult Index(int page = 1, int pageSize = 5)
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

        [HttpGet("/Product/LoadProductsPartial")]
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

            return PartialView("~/Views/Product/_ProductListPartial.cshtml", model);
        }




        // -------------------------------------------------------------------------------------------------------------------------





        [HttpGet("/Product/CreateProducts")]
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

                    using (var cmd = new OracleCommand("add_product", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Truyền tham số đầu vào cho stored procedure
                        cmd.Parameters.Add("p_product_id", OracleDbType.Int32).Value = product.ProductId;
                        cmd.Parameters.Add("p_product_name", OracleDbType.Varchar2).Value = product.ProductName;
                        cmd.Parameters.Add("p_category", OracleDbType.Varchar2).Value = product.ProductCategory;
                        cmd.Parameters.Add("p_price", OracleDbType.Decimal).Value = product.ProductPrice;
                        cmd.Parameters.Add("p_stock", OracleDbType.Int32).Value = product.ProductStock;

                        // Tham số OUT để nhận message từ procedure
                        var messageParam = new OracleParameter("p_message", OracleDbType.Varchar2, 4000)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(messageParam);

                        // Thực thi stored procedure
                        cmd.ExecuteNonQuery();

                        // Lấy thông điệp trả về
                        string resultMessage = messageParam.Value?.ToString();

                        if (resultMessage.Contains("Them san pham thanh cong!"))
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
        public IActionResult EditProduct(Product product, int page = 1)
        {
            if (!ModelState.IsValid)
                return View(product);

            try
            {
                using (var conn = new OracleConnection(_connectionString))
                {
                    conn.Open();

                    using (var cmd = new OracleCommand("update_product", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Các tham số đầu vào
                        cmd.Parameters.Add("p_product_id", OracleDbType.Int32).Value = product.ProductId;
                        cmd.Parameters.Add("p_product_name", OracleDbType.Varchar2).Value = product.ProductName;
                        cmd.Parameters.Add("p_category", OracleDbType.Varchar2).Value = product.ProductCategory;
                        cmd.Parameters.Add("p_price", OracleDbType.Decimal).Value = product.ProductPrice;
                        cmd.Parameters.Add("p_stock", OracleDbType.Int32).Value = product.ProductStock;

                        // Tham số đầu ra để lấy thông báo từ stored procedure
                        var messageParam = new OracleParameter("p_message", OracleDbType.Varchar2, 4000)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(messageParam);

                        // Thực thi stored procedure
                        cmd.ExecuteNonQuery();

                        // Lấy thông báo kết quả
                        string resultMessage = messageParam.Value?.ToString();

                        if (!string.IsNullOrEmpty(resultMessage) && resultMessage.Contains("Cap nhat thong tin san pham thanh cong!"))
                        {
                            TempData["Success"] = resultMessage;
                            return RedirectToAction("Index", new { page = page });
                        }
                        else
                        {
                            ModelState.AddModelError("", resultMessage ?? "Đã xảy ra lỗi không xác định.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Lỗi cập nhật: " + ex.Message);
            }

            return View(product);
        }



        // --------------------------------------------------------------------------------------------------------------------------------


        [HttpPost]
        public IActionResult DeleteProduct(int id, int page = 1)
        {
            try
            {
                using (var conn = new OracleConnection(_connectionString))
                {
                    conn.Open();

                    using (var cmd = new OracleCommand("delete_product", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Tham số đầu vào
                        cmd.Parameters.Add("p_product_id", OracleDbType.Int32).Value = id;

                        // Tham số OUT để nhận message từ procedure
                        var messageParam = new OracleParameter("p_message", OracleDbType.Varchar2, 4000)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(messageParam);

                        // Thực thi procedure
                        cmd.ExecuteNonQuery();

                        // Lấy thông báo trả về
                        string resultMessage = messageParam.Value?.ToString();

                        // Gán thông báo vào TempData để hiển thị
                        if (!string.IsNullOrEmpty(resultMessage) && resultMessage.Contains("Xoa san pham thanh cong!"))
                        {
                            TempData["Success"] = resultMessage;
                            return RedirectToAction("Index", new { page = page });
                        }
                        else
                        {
                            TempData["Error"] = resultMessage;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Lỗi khi xóa: " + ex.Message;
            }

            return RedirectToAction("Index", new { page = page });
        }


    }
}
