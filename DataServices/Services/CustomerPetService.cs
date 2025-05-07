// using MVC.Models;
// using Oracle.ManagedDataAccess.Client;
// using System;
// using System.Collections.Generic;
// using System.Data;
// using Microsoft.Extensions.Configuration;

// namespace MVC.Services
// {
//     public class CustomerPetService : ICustomerPetService
//     {
//         private readonly string _connectionString;

//         public CustomerPetService(IConfiguration configuration)
//         {
//             _connectionString = Environment.GetEnvironmentVariable("ORACLE_CONN_STRING")
//                                 ?? configuration.GetConnectionString("OracleConnection");
//         }

//         public List<CustomerPet> GetAllCustomerPets()
//         {
//             var CustomerPets = new List<CustomerPet>();

//             using (var conn = new OracleConnection(_connectionString))
//             {
//                 conn.Open();
//                 var cmd = new OracleCommand("SELECT a.customer_pet_id, a.customer_id, a.pet_id, a.purchase_date, a.price_at_purchase FROM customer_pet a order by a.customer_pet_id asc", conn);
//                 using var reader = cmd.ExecuteReader();

//                 while (reader.Read())
//                 {
//                     CustomerPets.Add(ReadCustomerPet(reader));
//                 }
//             }

//             return CustomerPets;
//         }

//         public List<CustomerPet> SearchCustomerPets(string keyword)
//         {
//             var CustomerPets = new List<CustomerPet>();
//             using (var conn = new OracleConnection(_connectionString))
//             {
//                 conn.Open();
//                 string query = "SELECT a.customer_pet_id, a.customer_id, a.pet_id, a.purchase_date, a.price_at_purchase FROM customer_pet  WHERE LOWER(last_name) LIKE :keyword ORDER BY CustomerPet_id ASC";
//                 var cmd = new OracleCommand(query, conn);
//                 cmd.Parameters.Add(new OracleParameter("keyword", $"%{keyword.ToLower()}%"));

//                 using var reader = cmd.ExecuteReader();
//                 while (reader.Read())
//                 {
//                     CustomerPets.Add(ReadCustomerPet(reader));
//                 }
//             }

//             return CustomerPets;
//         }

//         public CustomerPet GetCustomerPetById(int id)
//         {
//             using var conn = new OracleConnection(_connectionString);
//             conn.Open();

//             var cmd = new OracleCommand("SELECT CustomerPet_id, first_name, last_name, phone_number, email, address FROM CustomerPet WHERE CustomerPet_id = :CustomerPetId", conn);
//             cmd.Parameters.Add(new OracleParameter("CustomerPetId", id));

//             using var reader = cmd.ExecuteReader();
//             return reader.Read() ? ReadCustomerPet(reader) : null;
//         }

//         public string AddCustomerPet(CustomerPet CustomerPet)
//         {
//             using var conn = new OracleConnection(_connectionString);
//             conn.Open();

//             var cmd = new OracleCommand("add_CustomerPet", conn)
//             {
//                 CommandType = CommandType.StoredProcedure
//             };
//             cmd.Parameters.Add(new OracleParameter("p_first_name", CustomerPet.FirstName));
//             cmd.Parameters.Add(new OracleParameter("p_last_name", CustomerPet.LastName));
//             cmd.Parameters.Add(new OracleParameter("p_phone_number", CustomerPet.PhoneNumber));
//             cmd.Parameters.Add(new OracleParameter("p_email", CustomerPet.Email));
//             cmd.Parameters.Add(new OracleParameter("p_address", CustomerPet.Address));

//             var messageParam = new OracleParameter("p_message", OracleDbType.Varchar2, 4000)
//             {
//                 Direction = ParameterDirection.Output
//             };
//             cmd.Parameters.Add(messageParam);

//             cmd.ExecuteNonQuery();
//             return messageParam.Value.ToString();
//         }

//         public string UpdateCustomerPet(CustomerPet CustomerPet)
//         {
//             using var conn = new OracleConnection(_connectionString);
//             conn.Open();

//             var cmd = new OracleCommand("update_CustomerPet", conn)
//             {
//                 CommandType = CommandType.StoredProcedure
//             };

//             cmd.Parameters.Add("p_CustomerPet_id", OracleDbType.Int32).Value = CustomerPet.CustomerPetId;
//             cmd.Parameters.Add("p_first_name", OracleDbType.Varchar2).Value = CustomerPet.FirstName;
//             cmd.Parameters.Add("p_last_name", OracleDbType.Varchar2).Value = CustomerPet.LastName;
//             cmd.Parameters.Add("p_phone_number", OracleDbType.Varchar2).Value = CustomerPet.PhoneNumber;
//             cmd.Parameters.Add("p_email", OracleDbType.Varchar2).Value = CustomerPet.Email;
//             cmd.Parameters.Add("p_address", OracleDbType.Varchar2).Value = CustomerPet.Address;

//             var messageParam = new OracleParameter("p_message", OracleDbType.Varchar2, 4000)
//             {
//                 Direction = ParameterDirection.Output
//             };
//             cmd.Parameters.Add(messageParam);

//             cmd.ExecuteNonQuery();
//             return messageParam.Value.ToString();
//         }

//         public string DeleteCustomerPet(int id)
//         {
//             using var conn = new OracleConnection(_connectionString);
//             conn.Open();

//             var cmd = new OracleCommand("delete_CustomerPet", conn)
//             {
//                 CommandType = CommandType.StoredProcedure
//             };

//             cmd.Parameters.Add(new OracleParameter("p_CustomerPet_id", id));

//             var messageParam = new OracleParameter("p_message", OracleDbType.Varchar2, 4000)
//             {
//                 Direction = ParameterDirection.Output
//             };
//             cmd.Parameters.Add(messageParam);

//             cmd.ExecuteNonQuery();
//             return messageParam.Value.ToString();
//         }

//         private CustomerPet ReadCustomerPet(OracleDataReader reader)
//         {
//             return new CustomerPet
//             {
//                 CustomerPetId = reader.GetInt32(0),
//                 CustomerId = reader.GetInt32(1),
//                 PetId = reader.GetInt32(2),
//                 PurchaseDate = reader.GetDateTime(3),
//                 PriceAtPurchase = reader.GetDecimal(4),
//             };
//         }
//     }
// }
