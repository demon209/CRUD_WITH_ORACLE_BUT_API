using MVC.Models;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Extensions.Configuration;

namespace MVC.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly string _connectionString;

        public CustomerService(IConfiguration configuration)
        {
            _connectionString = Environment.GetEnvironmentVariable("ORACLE_CONN_STRING")
                                ?? configuration.GetConnectionString("OracleConnection");
        }

        public List<Customer> GetAllCustomers()
        {
            var Customers = new List<Customer>();

            using (var conn = new OracleConnection(_connectionString))
            {
                conn.Open();
                var cmd = new OracleCommand("SELECT customer_id, first_name, last_name, phone_number, email, address FROM customer order by customer_id asc", conn);
                using var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Customers.Add(ReadCustomer(reader));
                }
            }

            return Customers;
        }

        public List<Customer> SearchCustomers(string keyword)
        {
            var Customers = new List<Customer>();
            using (var conn = new OracleConnection(_connectionString))
            {
                conn.Open();
                string query = "SELECT customer_id, first_name, last_name, phone_number, email, address FROM customer WHERE LOWER(last_name) LIKE :keyword ORDER BY customer_id ASC";
                var cmd = new OracleCommand(query, conn);
                cmd.Parameters.Add(new OracleParameter("keyword", $"%{keyword.ToLower()}%"));

                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Customers.Add(ReadCustomer(reader));
                }
            }

            return Customers;
        }

        public Customer GetCustomerById(int id)
        {
            using var conn = new OracleConnection(_connectionString);
            conn.Open();

            var cmd = new OracleCommand("SELECT customer_id, first_name, last_name, phone_number, email, address FROM customer WHERE customer_id = :CustomerId", conn);
            cmd.Parameters.Add(new OracleParameter("CustomerId", id));

            using var reader = cmd.ExecuteReader();
            return reader.Read() ? ReadCustomer(reader) : null;
        }

        public string AddCustomer(Customer customer)
        {
            using var conn = new OracleConnection(_connectionString);
            conn.Open();

            var cmd = new OracleCommand("add_customer", conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.Add(new OracleParameter("p_first_name", customer.FirstName));
            cmd.Parameters.Add(new OracleParameter("p_last_name", customer.LastName));
            cmd.Parameters.Add(new OracleParameter("p_phone_number", customer.PhoneNumber));
            cmd.Parameters.Add(new OracleParameter("p_email", customer.Email));
            cmd.Parameters.Add(new OracleParameter("p_address", customer.Address));

            var messageParam = new OracleParameter("p_message", OracleDbType.Varchar2, 4000)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(messageParam);

            cmd.ExecuteNonQuery();
            return messageParam.Value.ToString();
        }

        public string UpdateCustomer(Customer customer)
        {
            using var conn = new OracleConnection(_connectionString);
            conn.Open();

            var cmd = new OracleCommand("update_customer", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.Add("p_customer_id", OracleDbType.Int32).Value = customer.CustomerId;
            cmd.Parameters.Add("p_first_name", OracleDbType.Varchar2).Value = customer.FirstName;
            cmd.Parameters.Add("p_last_name", OracleDbType.Varchar2).Value = customer.LastName;
            cmd.Parameters.Add("p_phone_number", OracleDbType.Varchar2).Value = customer.PhoneNumber;
            cmd.Parameters.Add("p_email", OracleDbType.Varchar2).Value = customer.Email;
            cmd.Parameters.Add("p_address", OracleDbType.Varchar2).Value = customer.Address;

            var messageParam = new OracleParameter("p_message", OracleDbType.Varchar2, 4000)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(messageParam);

            cmd.ExecuteNonQuery();
            return messageParam.Value.ToString();
        }

        public string DeleteCustomer(int id)
        {
            using var conn = new OracleConnection(_connectionString);
            conn.Open();

            var cmd = new OracleCommand("delete_customer", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.Add(new OracleParameter("p_customer_id", id));

            var messageParam = new OracleParameter("p_message", OracleDbType.Varchar2, 4000)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(messageParam);

            cmd.ExecuteNonQuery();
            return messageParam.Value.ToString();
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
