using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Collections.Generic;
using System;
using MVC.Models;  // Đảm bảo thêm đúng namespace cho Pet model

namespace YourNamespace.Controllers
{

    public class PetController : Controller
    {
        private readonly string _connectionString = Environment.GetEnvironmentVariable("ORACLE_CONN_STRING");

        [HttpGet]
        public IActionResult GetPets()
        {
            List<Pet> pets = new List<Pet>();

            using (var conn = new OracleConnection(_connectionString))
            {
                try
                {
                    conn.Open();
                    using (var cmd = new OracleCommand("SELECT pet_id, pet_name, pet_type, breed, age, gender, price, stock FROM usertest.pet", conn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Console.WriteLine($"PetId: {reader.GetInt32(1)}, PetName: {reader.GetString(2)}");
                                pets.Add(new Pet
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
                catch (Exception ex)
                {
                    return StatusCode(500, "Internal server error: " + ex.Message);
                }
            }

            return Ok(pets);
        }
        [HttpGet("/Thucung")]
        public IActionResult Index(int page = 1, int pageSize = 5)
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

        [HttpGet("/Pet/LoadPetsPartial")]
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

            return PartialView("~/Views/Pet/_PetListPartial.cshtml", model);
        }

        [HttpGet("Pet/CreatePets")]
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

                    // Sử dụng stored procedure và chỉ định kiểu CommandType
                    using (var cmd = new OracleCommand("add_pet", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Thêm các tham số vào stored procedure
                        cmd.Parameters.Add(new OracleParameter("PetId", pet.PetId));
                        cmd.Parameters.Add(new OracleParameter("PetName", pet.PetName));
                        cmd.Parameters.Add(new OracleParameter("PetType", pet.PetType));
                        cmd.Parameters.Add(new OracleParameter("Breed", pet.Breed));
                        cmd.Parameters.Add(new OracleParameter("Age", pet.Age));
                        cmd.Parameters.Add(new OracleParameter("Gender", pet.Gender));
                        cmd.Parameters.Add(new OracleParameter("Price", pet.Price));
                        cmd.Parameters.Add(new OracleParameter("Stock", pet.Stock));

                        // Thêm OUT parameter để nhận thông báo từ stored procedure
                        var messageParam = new OracleParameter("p_message", OracleDbType.Varchar2, 4000)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(messageParam);

                        // Thực thi stored procedure
                        cmd.ExecuteNonQuery();

                        // Lấy thông báo từ OUT parameter
                        string message = messageParam.Value.ToString();

                        // Kiểm tra thông báo và hiển thị
                        if (message.Contains("Them thu cung thanh cong!"))
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
            return View(pet);
        }

        [HttpPost]
        public IActionResult DeletePet(int id, int page = 1)
        {
            try
            {
                using (var conn = new OracleConnection(_connectionString))
                {
                    conn.Open();

                    // Tạo đối tượng OracleCommand để gọi thủ tục delete_pet
                    using (var cmd = new OracleCommand("delete_pet", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Thêm tham số đầu vào
                        cmd.Parameters.Add(new OracleParameter("p_pet_id", id));

                        // Thêm tham số đầu ra để nhận thông báo
                        var messageParam = new OracleParameter("p_message", OracleDbType.Varchar2, 4000)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(messageParam);

                        // Thực thi thủ tục
                        cmd.ExecuteNonQuery();

                        // Lấy thông báo từ tham số đầu ra
                        string message = messageParam.Value.ToString();

                        // Kiểm tra thông báo và xử lý
                        if (message.Contains("Xoa thu cung thanh cong!"))
                        {
                            TempData["Success"] = message;
                            return RedirectToAction("Index", new { page = page });
                        }
                        else
                        {
                            ModelState.AddModelError("", message);
                        }
                    }

                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Lỗi khi xóa: " + ex.Message; // Thông báo lỗi nếu có lỗi ngoài thủ tục
            }

            // Sau khi xóa xong, chuyển hướng về trang danh sách thú cưng với số trang hiện tại
            return RedirectToAction("Index", new { page = page });
        }

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
        public IActionResult EditPet(Pet pet, int page = 1)
        {
            if (!ModelState.IsValid)
                return View(pet);

            try
            {
                using (var conn = new OracleConnection(_connectionString))
                {
                    conn.Open();

                    // Tạo đối tượng OracleCommand để gọi thủ tục update_pet
                    using (var cmd = new OracleCommand("update_pet", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Thêm các tham số đầu vào
                        cmd.Parameters.Add(new OracleParameter("p_pet_id", pet.PetId));
                        cmd.Parameters.Add(new OracleParameter("p_pet_name", pet.PetName));
                        cmd.Parameters.Add(new OracleParameter("p_pet_type", pet.PetType));
                        cmd.Parameters.Add(new OracleParameter("p_breed", pet.Breed));
                        cmd.Parameters.Add(new OracleParameter("p_age", pet.Age));
                        cmd.Parameters.Add(new OracleParameter("p_gender", pet.Gender));
                        cmd.Parameters.Add(new OracleParameter("p_price", pet.Price));
                        cmd.Parameters.Add(new OracleParameter("p_stock", pet.Stock));

                        // Thêm tham số đầu ra để nhận thông báo
                        var messageParam = new OracleParameter("p_message", OracleDbType.Varchar2, 4000)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(messageParam);

                        // Thực thi thủ tục
                        cmd.ExecuteNonQuery();

                        // Lấy thông báo từ tham số đầu ra
                        string message = messageParam.Value.ToString();

                        // Hiển thị thông báo thành công hoặc lỗi
                        if (message.Contains("Cap nhat thong tin thu cung thanh cong!"))
                        {
                            TempData["Success"] = message;
                            return RedirectToAction("Index", new { page = page });
                        }
                        else
                        {
                            ModelState.AddModelError("", message);
                        }
                    }

                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Lỗi khi cập nhật: " + ex.Message;
            }

            return RedirectToAction("Index", new { page = page });
        }

    }
}
