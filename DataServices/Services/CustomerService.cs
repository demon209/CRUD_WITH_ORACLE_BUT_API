using MVC.Models;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Extensions.Configuration;

namespace MVC.Services
{
    public class CustomerService : CrudService<Customer>, ICustomerService
    {
        private const string BaseSelect = "SELECT customer_id, first_name, last_name, phone_number, email, address FROM customer";

        public CustomerService(IConfiguration configuration) : base(configuration) { }

        public override List<Customer> GetAll()
        {
            var customers = new List<Customer>();
            using var conn = new OracleConnection(_connectionString);
            conn.Open();

            var cmd = new OracleCommand($"{BaseSelect} ORDER BY customer_id ASC", conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                customers.Add(ReadCustomer(reader));
            }

            return customers;
        }

        public override Customer GetById(int id)
        {
            using var conn = new OracleConnection(_connectionString);
            conn.Open();

            var cmd = new OracleCommand($"{BaseSelect} WHERE customer_id = :CustomerId", conn);
            cmd.Parameters.Add(new OracleParameter("CustomerId", id));

            using var reader = cmd.ExecuteReader();
            return reader.Read() ? ReadCustomer(reader) : null;
        }

        public override string Add(Customer customer)
        {
            var inputParams = new Dictionary<string, object>
            {
                { "p_first_name", customer.FirstName },
                { "p_last_name", customer.LastName },
                { "p_phone_number", customer.PhoneNumber },
                { "p_email", customer.Email },
                { "p_address", customer.Address }
            };

            return ExecuteStoredProcedure("add_customer", inputParams, "p_message");
        }

        public override string Update(Customer customer)
        {
            var inputParams = new Dictionary<string, object>
            {
                { "p_customer_id", customer.CustomerId },
                { "p_first_name", customer.FirstName },
                { "p_last_name", customer.LastName },
                { "p_phone_number", customer.PhoneNumber },
                { "p_email", customer.Email },
                { "p_address", customer.Address }
            };

            return ExecuteStoredProcedure("update_customer", inputParams, "p_message");
        }

        public override string Delete(int id)
        {
            var inputParams = new Dictionary<string, object>
            {
                { "p_customer_id", id }
            };

            return ExecuteStoredProcedure("delete_customer", inputParams, "p_message");
        }

        public List<Customer> SearchCustomers(string keyword)
        {
            var customers = new List<Customer>();
            using var conn = new OracleConnection(_connectionString);
            conn.Open();

            var cmd = new OracleCommand("search_customer", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.Add("p_keyword", OracleDbType.Varchar2).Value = keyword;
            cmd.Parameters.Add("p_result", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                customers.Add(ReadCustomer(reader));
            }

            return customers;
        }

        private Customer ReadCustomer(OracleDataReader reader)
        {
            return new Customer
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
