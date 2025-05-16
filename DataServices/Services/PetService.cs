using MVC.Models;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Extensions.Configuration;

namespace MVC.Services
{
    public class PetService : CrudService<Pet>, IPetService
    {
        public PetService(IConfiguration configuration) : base(configuration) { }

        public override List<Pet> GetAll()
        {
            var pets = new List<Pet>();
            using var conn = new OracleConnection(_connectionString);
            conn.Open();

            var cmd = new OracleCommand("SELECT * FROM pet ORDER BY pet_id ASC", conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                pets.Add(ReadPet(reader));
            }

            return pets;
        }

        public List<Pet> SearchPets(string keyword)
        {
            var pets = new List<Pet>();
            using var conn = new OracleConnection(_connectionString);
            conn.Open();

            var cmd = new OracleCommand("search_pet", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.Add("p_keyword", OracleDbType.Varchar2).Value = keyword;
            cmd.Parameters.Add("p_result", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                pets.Add(ReadPet(reader));
            }

            return pets;
        }

        public override Pet GetById(int id)
        {
            using var conn = new OracleConnection(_connectionString);
            conn.Open();

            var cmd = new OracleCommand("SELECT * FROM pet WHERE pet_id = :PetId", conn);
            cmd.Parameters.Add(new OracleParameter("PetId", id));

            using var reader = cmd.ExecuteReader();
            return reader.Read() ? ReadPet(reader) : null;
        }

        // Không dùng ảnh => throw để tránh nhầm
        public override string Add(Pet entity) => throw new NotImplementedException("Use Add with imageData for Pet.");
        public override string Update(Pet entity) => throw new NotImplementedException("Use Update with imageData for Pet.");

        // Dùng ảnh
        public string Add(Pet pet, byte[] imageData)
        {
            try
            {
                if (imageData == null || imageData.Length == 0)
                {
                    // Gán thẳng ảnh mặc định
                    imageData = File.ReadAllBytes("wwwroot/image/notfound.png");
                }

                using var conn = new OracleConnection(_connectionString);
                conn.Open();

                using var cmd = new OracleCommand("add_pet", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.Add("p_pet_name", OracleDbType.Varchar2).Value = pet.PetName ?? "";
                cmd.Parameters.Add("p_pet_type", OracleDbType.Varchar2).Value = pet.PetType ?? "";
                cmd.Parameters.Add("p_breed", OracleDbType.Varchar2).Value = pet.Breed ?? "";
                cmd.Parameters.Add("p_age", OracleDbType.Int32).Value = pet.Age;
                cmd.Parameters.Add("p_gender", OracleDbType.Varchar2).Value = pet.Gender ?? "";
                cmd.Parameters.Add("p_price", OracleDbType.Decimal).Value = pet.Price;
                cmd.Parameters.Add("p_image", OracleDbType.Blob).Value = imageData;
                cmd.Parameters.Add("p_message", OracleDbType.Varchar2, 4000).Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();
                return cmd.Parameters["p_message"].Value.ToString();
            }
            catch (Exception ex)
            {
                return "ERROR: " + ex.Message;
            }
        }


        public string Update(Pet pet, byte[] imageData)
        {
            using var conn = new OracleConnection(_connectionString);
            conn.Open();

            var cmd = new OracleCommand("update_pet", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.Add("p_pet_id", OracleDbType.Int32).Value = pet.PetId;
            cmd.Parameters.Add("p_pet_name", OracleDbType.Varchar2).Value = pet.PetName;
            cmd.Parameters.Add("p_pet_type", OracleDbType.Varchar2).Value = pet.PetType;
            cmd.Parameters.Add("p_breed", OracleDbType.Varchar2).Value = pet.Breed;
            cmd.Parameters.Add("p_age", OracleDbType.Int32).Value = pet.Age;
            cmd.Parameters.Add("p_gender", OracleDbType.Varchar2).Value = pet.Gender;
            cmd.Parameters.Add("p_price", OracleDbType.Decimal).Value = pet.Price;
            cmd.Parameters.Add("p_status", OracleDbType.Varchar2).Value = pet.Status;

            if (imageData == null || imageData.Length == 0)
                cmd.Parameters.Add("p_image", OracleDbType.Blob).Value = DBNull.Value;
            else
                cmd.Parameters.Add("p_image", OracleDbType.Blob).Value = imageData;

            cmd.Parameters.Add("p_message", OracleDbType.Varchar2, 4000).Direction = ParameterDirection.Output;
            cmd.ExecuteNonQuery();

            return cmd.Parameters["p_message"].Value.ToString();
        }

        public override string Delete(int orderId)
        {
            var inputParams = new Dictionary<string, object>
            {
                { "p_pet_id", orderId }
            };

            return ExecuteStoredProcedure("delete_pet", inputParams, "p_message");
        }

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
                ImageData = reader.IsDBNull(8) ? null : reader.GetOracleBlob(8).Value
            };
        }
    }
}
