using MVC.Models;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Extensions.Configuration;

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

        public List<Pet> GetAllPets()
        {
            var pets = new List<Pet>();

            using (var conn = new OracleConnection(_connectionString))
            {
                conn.Open();
                var cmd = new OracleCommand("SELECT pet_id, pet_name, pet_type, breed, age, gender, price, stock FROM pet ORDER BY pet_id ASC", conn);
                using var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    pets.Add(ReadPet(reader));
                }
            }

            return pets;
        }

        public List<Pet> SearchPets(string keyword)
        {
            var pets = new List<Pet>();
            using (var conn = new OracleConnection(_connectionString))
            {
                conn.Open();
                string query = "SELECT pet_id, pet_name, pet_type, breed, age, gender, price, stock FROM pet WHERE LOWER(pet_name) LIKE :keyword ORDER BY pet_id ASC";
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

        public Pet GetPetById(int id)
        {
            using var conn = new OracleConnection(_connectionString);
            conn.Open();

            var cmd = new OracleCommand("SELECT pet_id, pet_name, pet_type, breed, age, gender, price, stock FROM pet WHERE pet_id = :PetId", conn);
            cmd.Parameters.Add(new OracleParameter("PetId", id));

            using var reader = cmd.ExecuteReader();
            return reader.Read() ? ReadPet(reader) : null;
        }

        public string AddPet(Pet pet)
        {
            using var conn = new OracleConnection(_connectionString);
            conn.Open();

            var cmd = new OracleCommand("add_pet", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.Add(new OracleParameter("PetName", pet.PetName));
            cmd.Parameters.Add(new OracleParameter("PetType", pet.PetType));
            cmd.Parameters.Add(new OracleParameter("Breed", pet.Breed));
            cmd.Parameters.Add(new OracleParameter("Age", pet.Age));
            cmd.Parameters.Add(new OracleParameter("Gender", pet.Gender));
            cmd.Parameters.Add(new OracleParameter("Price", pet.Price));
            cmd.Parameters.Add(new OracleParameter("Stock", pet.Stock));

            var messageParam = new OracleParameter("p_message", OracleDbType.Varchar2, 4000)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(messageParam);

            cmd.ExecuteNonQuery();
            return messageParam.Value.ToString();
        }

        public string UpdatePet(Pet pet)
        {
            using var conn = new OracleConnection(_connectionString);
            conn.Open();

            var cmd = new OracleCommand("update_pet", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.Add(new OracleParameter("p_pet_id", pet.PetId));
            cmd.Parameters.Add(new OracleParameter("p_pet_name", pet.PetName));
            cmd.Parameters.Add(new OracleParameter("p_pet_type", pet.PetType));
            cmd.Parameters.Add(new OracleParameter("p_breed", pet.Breed));
            cmd.Parameters.Add(new OracleParameter("p_age", pet.Age));
            cmd.Parameters.Add(new OracleParameter("p_gender", pet.Gender));
            cmd.Parameters.Add(new OracleParameter("p_price", pet.Price));
            cmd.Parameters.Add(new OracleParameter("p_stock", pet.Stock));

            var messageParam = new OracleParameter("p_message", OracleDbType.Varchar2, 4000)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(messageParam);

            cmd.ExecuteNonQuery();
            return messageParam.Value.ToString();
        }

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
            return messageParam.Value.ToString();
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
                Stock = reader.GetInt32(7)
            };
        }
    }
}
