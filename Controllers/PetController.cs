using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
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
                            TempData["Success"] = "Them thu cung thanh cong!";
                            return RedirectToAction("Thucung");
                        }
                        else
                        {
                            ModelState.AddModelError("", "Thêm thất bại.");
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
                string deleteQuery = "DELETE FROM usertest.pet WHERE pet_id = :PetId";
                using (var cmd = new OracleCommand(deleteQuery, conn))
                {
                    cmd.Parameters.Add(new OracleParameter("PetId", id));
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        TempData["Success"] = "Xoa thu cung thanh cong!";
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
                        TempData["Success"] = " Cap nhat thu cung thanh cong!";
                        return RedirectToAction("Index", new { page = page });
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
    }
}
