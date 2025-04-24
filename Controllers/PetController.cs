using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System;
using MVC.Models;  // Đảm bảo thêm đúng namespace cho Pet model

namespace YourNamespace.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PetController : ControllerBase
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
    }
}
