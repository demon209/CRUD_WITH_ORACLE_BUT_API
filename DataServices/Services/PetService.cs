using MVC.Models;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace MVC.Services
{
    public class PetService : IPetService
    {
        private readonly string _connectionString;

        public PetService(IConfiguration configuration)
        {
            _connectionString = Environment.GetEnvironmentVariable("ORACLE_CONN_STRING")
                                ?? configuration.GetConnectionString("OracleConnection");
        }

        // Lấy tất cả thú cưng
        public List<Pet> GetAllPets()
        {
            var pets = new List<Pet>();

            using (var conn = new OracleConnection(_connectionString))
            {
                conn.Open();
                var cmd = new OracleCommand("SELECT pet_id, pet_name, pet_type, breed, age, gender, price, status, image FROM pet ORDER BY pet_id ASC", conn);
                using var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    pets.Add(ReadPet(reader));
                }
            }

            return pets;
        }

        // Tìm kiếm thú cưng
        public List<Pet> SearchPets(string keyword)
        {
            var pets = new List<Pet>();
            using (var conn = new OracleConnection(_connectionString))
            {
                conn.Open();
                string query = "SELECT pet_id, pet_name, pet_type, breed, age, gender, price, status, image FROM pet WHERE LOWER(pet_name) LIKE :keyword ORDER BY pet_id ASC";
                var cmd = new OracleCommand(query, conn);
                cmd.Parameters.Add(new OracleParameter("keyword", $"%{keyword.ToLower()}%"));

                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    pets.Add(ReadPet(reader));
                }
            }

            return pets;
        }

        // Lấy thông tin thú cưng theo ID
        public Pet GetPetById(int id)
        {
            using var conn = new OracleConnection(_connectionString);
            conn.Open();

            var cmd = new OracleCommand("SELECT pet_id, pet_name, pet_type, breed, age, gender, price, status, image FROM pet WHERE pet_id = :PetId", conn);
            cmd.Parameters.Add(new OracleParameter("PetId", id));

            using var reader = cmd.ExecuteReader();
            return reader.Read() ? ReadPet(reader) : null;
        }

        // Thêm thú cưng mới với ảnh dưới dạng byte[]
        public string AddPet(Pet pet, byte[] imageData)
        {
            try
            {
                using (var conn = new OracleConnection(_connectionString))
                {
                    conn.Open();
                    using (var cmd = new OracleCommand("add_pet", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Input parameters
                        cmd.Parameters.Add("p_pet_name", OracleDbType.Varchar2).Value = pet.PetName ?? "";
                        cmd.Parameters.Add("p_pet_type", OracleDbType.Varchar2).Value = pet.PetType ?? "";
                        cmd.Parameters.Add("p_breed", OracleDbType.Varchar2).Value = pet.Breed ?? "";
                        cmd.Parameters.Add("p_age", OracleDbType.Int32).Value = pet.Age;
                        cmd.Parameters.Add("p_gender", OracleDbType.Varchar2).Value = pet.Gender ?? "";
                        cmd.Parameters.Add("p_price", OracleDbType.Decimal).Value = pet.Price;
                        cmd.Parameters.Add("p_image", OracleDbType.Blob).Value = imageData ?? new byte[0]; // Handle null image

                        // Output parameter
                        cmd.Parameters.Add("p_message", OracleDbType.Varchar2, 4000).Direction = ParameterDirection.Output;

                        // Execute the command
                        cmd.ExecuteNonQuery();

                        // Return the output message
                        return cmd.Parameters["p_message"].Value.ToString();
                    }
                }
            }
            catch (OracleException ex)
            {
                // Log the error for debugging
                Console.WriteLine(ex.Message);
                return "ERROR: " + ex.Message;
            }
            catch (Exception ex)
            {
                // Handle other exceptions
                Console.WriteLine(ex.Message);
                return "ERROR: " + ex.Message;
            }
        }

        // Cập nhật thú cưng với ảnh dưới dạng byte[]
        public string UpdatePet(Pet pet, byte[] imageData)
        {
            using var conn = new OracleConnection(_connectionString);
            conn.Open();

            var cmd = new OracleCommand("update_pet", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            // Thêm các tham số khác
            cmd.Parameters.Add("p_pet_id", OracleDbType.Int32).Value = pet.PetId;
            cmd.Parameters.Add("p_pet_name", OracleDbType.Varchar2).Value = pet.PetName;
            cmd.Parameters.Add("p_pet_type", OracleDbType.Varchar2).Value = pet.PetType;
            cmd.Parameters.Add("p_breed", OracleDbType.Varchar2).Value = pet.Breed;
            cmd.Parameters.Add("p_age", OracleDbType.Int32).Value = pet.Age;
            cmd.Parameters.Add("p_gender", OracleDbType.Varchar2).Value = pet.Gender;
            cmd.Parameters.Add("p_price", OracleDbType.Decimal).Value = pet.Price;
            cmd.Parameters.Add("p_status", OracleDbType.Varchar2).Value = "Còn thú cưng";

            // Kiểm tra ảnh có được truyền không
            if (imageData == null || imageData.Length == 0)
            {
                cmd.Parameters.Add("p_image", OracleDbType.Blob).Value = DBNull.Value;  // Truyền DBNull.Value khi không có ảnh
            }
            else
            {
                cmd.Parameters.Add("p_image", OracleDbType.Blob).Value = imageData;  // Truyền ảnh nếu có
            }

            // Thêm tham số thông báo
            var messageParam = new OracleParameter("p_message", OracleDbType.Varchar2, 4000)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(messageParam);

            // Thực thi stored procedure
            cmd.ExecuteNonQuery();

            // Trả lại thông báo từ stored procedure
            return messageParam.Value.ToString();
        }






        // Xóa thú cưng
        public string DeletePet(int id)
        {
            using var conn = new OracleConnection(_connectionString);
            conn.Open();

            var cmd = new OracleCommand("delete_pet", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.Add(new OracleParameter("p_pet_id", id));

            var messageParam = new OracleParameter("p_message", OracleDbType.Varchar2, 4000)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(messageParam);

            cmd.ExecuteNonQuery();

            // Cập nhật lại trạng thái sau khi xóa thú cưng
            var updateStatusCmd = new OracleCommand("UPDATE pet SET status = 'Còn thú cưng' WHERE pet_id = :pet_id", conn);
            updateStatusCmd.Parameters.Add(new OracleParameter("pet_id", id));
            updateStatusCmd.ExecuteNonQuery();

            return messageParam.Value.ToString();
        }

        // Đọc dữ liệu thú cưng từ cơ sở dữ liệu
        private Pet ReadPet(OracleDataReader reader)
        {
            return new Pet
            {
                PetId = reader.GetInt32(0),
                PetName = reader.GetString(1),
                PetType = reader.GetString(2),
                Breed = reader.GetString(3),
                Age = reader.GetInt32(4),
                Gender = reader.GetString(5),
                Price = reader.GetDecimal(6),
                Status = reader.GetString(7),
                // Đọc ảnh dưới dạng byte[]
                ImageData = reader.IsDBNull(8) ? null : reader.GetOracleBlob(8).Value
            };
        }
    }
}
