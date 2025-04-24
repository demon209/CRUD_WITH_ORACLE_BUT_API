// using Oracle.ManagedDataAccess.Client;
// using System;
// using System.Collections.Generic;
// using MVC.Models;

// namespace MVC.Helper
// {
//     public class OracleHelper
//     {
//         public static List<Pet> GetPets()
//         {
//             var pets = new List<Pet>();
//             string connStr = Environment.GetEnvironmentVariable("ORACLE_CONN_STRING");

//             using (var conn = new OracleConnection(connStr))
//             {
//                 try
//                 {
//                     conn.Open();
//                     Console.WriteLine("✅ Đã mở kết nối!");

//                     string query = "SELECT pet_id, pet_name, pet_type, breed, age, gender, price, stock FROM usertest.pet";

//                     using (var cmd = new OracleCommand(query, conn))
//                     using (var reader = cmd.ExecuteReader())
//                     {
//                         int count = 0;
//                         while (reader.Read())
//                         {
//                             pets.Add(new Pet
//                             {
//                                 PetId = reader.GetInt32(0),
//                                 PetName = reader.GetString(1),
//                                 PetType = reader.GetString(2),
//                                 Breed = reader.GetString(3),
//                                 Age = reader.GetInt32(4),
//                                 Gender = reader.GetString(5),
//                                 Price = reader.GetDecimal(6),
//                                 Stock = reader.GetInt32(7)
//                             });
//                             count++;
//                         }

//                         Console.WriteLine($"Tổng số thú cưng: {count}");
//                     }
//                 }
//                 catch (Exception ex)
//                 {
//                     Console.WriteLine("❌ Lỗi: " + ex.Message);
//                 }
//             }

//             return pets;
//         }
//     }
// }


