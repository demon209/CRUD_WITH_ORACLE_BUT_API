using MVC.Models;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Extensions.Configuration;

namespace MVC.Services
{
    public class ProductService : IProductService
    {
        private readonly string _connectionString;

        public ProductService(IConfiguration configuration)
        {
            _connectionString = Environment.GetEnvironmentVariable("ORACLE_CONN_STRING")
                                ?? configuration.GetConnectionString("OracleConnection");
        }

        public List<Product> GetAllProducts()
        {
            var Products = new List<Product>();

            using (var conn = new OracleConnection(_connectionString))
            {
                conn.Open();
                var cmd = new OracleCommand("SELECT product_id, product_name, category, price, stock FROM product order by product_id asc", conn);
                using var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Products.Add(ReadProduct(reader));
                }
            }

            return Products;
        }

        public List<Product> SearchProducts(string keyword)
        {
            var Products = new List<Product>();
            using (var conn = new OracleConnection(_connectionString))
            {
                conn.Open();
                string query = "SELECT product_id, product_name, category, price, stock FROM product WHERE LOWER(product_name) LIKE :keyword ORDER BY product_id ASC";
                var cmd = new OracleCommand(query, conn);
                cmd.Parameters.Add(new OracleParameter("keyword", $"%{keyword.ToLower()}%"));

                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Products.Add(ReadProduct(reader));
                }
            }

            return Products;
        }

        public Product GetProductById(int id)
        {
            using var conn = new OracleConnection(_connectionString);
            conn.Open();

            var cmd = new OracleCommand("SELECT product_id, product_name, category, price, stock FROM product WHERE product_id = :ProductId", conn);
            cmd.Parameters.Add(new OracleParameter("ProductId", id));

            using var reader = cmd.ExecuteReader();
            return reader.Read() ? ReadProduct(reader) : null;
        }

        public string AddProduct(Product product)
        {
            using var conn = new OracleConnection(_connectionString);
            conn.Open();

            var cmd = new OracleCommand("add_product", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.Add("p_product_name", OracleDbType.Varchar2).Value = product.ProductName;
            cmd.Parameters.Add("p_category", OracleDbType.Varchar2).Value = product.ProductCategory;
            cmd.Parameters.Add("p_price", OracleDbType.Decimal).Value = product.ProductPrice;
            cmd.Parameters.Add("p_stock", OracleDbType.Int32).Value = product.ProductStock;

            var messageParam = new OracleParameter("p_message", OracleDbType.Varchar2, 4000)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(messageParam);

            cmd.ExecuteNonQuery();
            return messageParam.Value.ToString();
        }

        public string UpdateProduct(Product product)
        {
            using var conn = new OracleConnection(_connectionString);
            conn.Open();

            var cmd = new OracleCommand("update_product", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.Add("p_product_id", OracleDbType.Int32).Value = product.ProductId;
            cmd.Parameters.Add("p_product_name", OracleDbType.Varchar2).Value = product.ProductName;
            cmd.Parameters.Add("p_category", OracleDbType.Varchar2).Value = product.ProductCategory;
            cmd.Parameters.Add("p_price", OracleDbType.Decimal).Value = product.ProductPrice;
            cmd.Parameters.Add("p_stock", OracleDbType.Int32).Value = product.ProductStock;

            var messageParam = new OracleParameter("p_message", OracleDbType.Varchar2, 4000)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(messageParam);

            cmd.ExecuteNonQuery();
            return messageParam.Value.ToString();
        }

        public string DeleteProduct(int id)
        {
            using var conn = new OracleConnection(_connectionString);
            conn.Open();

            var cmd = new OracleCommand("delete_product", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.Add(new OracleParameter("p_product_id", id));

            var messageParam = new OracleParameter("p_message", OracleDbType.Varchar2, 4000)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(messageParam);

            cmd.ExecuteNonQuery();
            return messageParam.Value.ToString();
        }

        private Product ReadProduct(OracleDataReader reader)
        {
            return new Product
            {
                ProductId = reader.GetInt32(0),
                ProductName = reader.GetString(1),
                ProductCategory = reader.GetString(2),
                ProductPrice = reader.GetDecimal(3),
                ProductStock = reader.GetInt32(4),
            };
        }
    }
}
