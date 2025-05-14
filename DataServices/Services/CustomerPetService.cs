using MVC.Models;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Extensions.Configuration;

namespace MVC.Services
{
    public class CustomerPetService : CrudService<CustomerPet>, ICustomerPetService
    {
        private const string BaseSelect = @"
            SELECT 
                cp.customer_pet_id, 
                cp.customer_id, 
                cp.pet_name, 
                cp.product_id, 
                cp.status,
                c.last_name, 
                c.first_name,
                p.product_name
            FROM customer_pet cp
            JOIN customer c ON cp.customer_id = c.customer_id
            JOIN product p ON cp.product_id = p.product_id";

        public CustomerPetService(IConfiguration configuration) : base(configuration) { }

        // Phương thức lấy tất cả dữ liệu
        public override List<CustomerPet> GetAll()
        {
            var customerPets = new List<CustomerPet>();
            using var conn = new OracleConnection(_connectionString);
            conn.Open();

            var cmd = new OracleCommand($"{BaseSelect} ORDER BY cp.customer_pet_id ASC", conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                customerPets.Add(ReadCustomerPet(reader));
            }

            return customerPets;
        }

        // Phương thức lấy thông tin theo ID
        public override CustomerPet GetById(int id)
        {
            using var conn = new OracleConnection(_connectionString);
            conn.Open();

            var cmd = new OracleCommand($"{BaseSelect} WHERE cp.customer_pet_id = :CustomerPetId", conn);
            cmd.Parameters.Add(new OracleParameter("CustomerPetId", id));

            using var reader = cmd.ExecuteReader();
            return reader.Read() ? ReadCustomerPet(reader) : null;
        }

        // Phương thức thêm mới dịch vụ
        public override string Add(CustomerPet customerPet)
        {
            var inputParams = new Dictionary<string, object>
            {
                { "p_customer_id", customerPet.CustomerId },
                { "p_pet_name", customerPet.PetName },
                { "p_product_id", customerPet.ProductId },
                { "p_status", customerPet.Status ?? "Đang thực hiện" }
            };

            return ExecuteStoredProcedure("add_customerpet", inputParams, "p_message");
        }

        // Phương thức cập nhật dịch vụ
        public override string Update(CustomerPet customerPet)
        {
            var inputParams = new Dictionary<string, object>
            {
                { "p_customer_pet_id", customerPet.CustomerPetId },
                { "p_customer_id", customerPet.CustomerId },
                { "p_pet_name", customerPet.PetName },
                { "p_product_id", customerPet.ProductId },
                { "p_status", customerPet.Status ?? "Đang thực hiện" }
            };

            return ExecuteStoredProcedure("update_customerpet", inputParams, "p_message");
        }

        // Phương thức xóa dịch vụ
        public override string Delete(int id)
        {
            var inputParams = new Dictionary<string, object>
            {
                { "p_customer_pet_id", id }
            };

            return ExecuteStoredProcedure("delete_customerpet", inputParams, "p_message");
        }

        // Phương thức tìm kiếm dịch vụ
        public List<CustomerPet> SearchCustomerPet(string keyword)
        {
            var customerPets = new List<CustomerPet>();
            using var conn = new OracleConnection(_connectionString);
            conn.Open();

            var cmd = new OracleCommand("search_customerpet", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.Add("p_keyword", OracleDbType.Varchar2).Value = keyword;
            cmd.Parameters.Add("p_result", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                customerPets.Add(ReadCustomerPet(reader));
            }

            return customerPets;
        }
        public string? ToggleStatus(int id)
        {
            using var conn = new OracleConnection(_connectionString);
            conn.Open();

            var getCmd = new OracleCommand("SELECT status FROM customer_pet WHERE customer_pet_id = :id", conn);
            getCmd.Parameters.Add(new OracleParameter("id", id));
            var currentStatus = getCmd.ExecuteScalar()?.ToString();

            if (string.IsNullOrEmpty(currentStatus))
                return null;

            var newStatus = currentStatus == "Hoàn thành" ? "Đang thực hiện" : "Hoàn thành";

            var updateCmd = new OracleCommand("UPDATE customer_pet SET status = :newStatus WHERE customer_pet_id = :id", conn);
            updateCmd.Parameters.Add(new OracleParameter("newStatus", newStatus));
            updateCmd.Parameters.Add(new OracleParameter("id", id));
            updateCmd.ExecuteNonQuery();

            return newStatus;
        }


        // Đọc thông tin từ OracleDataReader
        private CustomerPet ReadCustomerPet(OracleDataReader reader)
        {
            return new CustomerPet
            {
                CustomerPetId = reader.GetInt32(reader.GetOrdinal("customer_pet_id")),
                CustomerId = reader.GetInt32(reader.GetOrdinal("customer_id")),
                PetName = reader["pet_name"]?.ToString(),
                ProductId = reader.GetInt32(reader.GetOrdinal("product_id")),
                LastName = reader["last_name"]?.ToString(), // Thêm CustomerName
                FirstName = reader["first_name"]?.ToString(),
                ProductName = reader["product_name"]?.ToString(), // Thêm ProductName
                Status = reader["status"]?.ToString() // 👈 Thêm dòng này
            };
        }
    }
}
