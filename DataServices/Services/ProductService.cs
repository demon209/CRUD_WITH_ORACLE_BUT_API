using MVC.Models;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Extensions.Configuration;

namespace MVC.Services
{
    public class ProductService : CrudService<Product>, IProductService
    {
        public ProductService(IConfiguration configuration) : base(configuration) { }

        public override List<Product> GetAll()
        {
            var products = new List<Product>();
            using var conn = new OracleConnection(_connectionString);
            conn.Open();

            var cmd = new OracleCommand("SELECT * FROM product ORDER BY product_id ASC", conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                products.Add(ReadProduct(reader));
            }

            return products;
        }

        public List<Product> SearchProducts(string keyword)
        {
            var products = new List<Product>();
            using var conn = new OracleConnection(_connectionString);
            conn.Open();

            var cmd = new OracleCommand("search_product", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.Add("p_keyword", OracleDbType.Varchar2).Value = keyword ?? string.Empty;
            cmd.Parameters.Add("p_result", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                products.Add(ReadProduct(reader));
            }

            return products;
        }

        public override Product GetById(int id)
        {
            using var conn = new OracleConnection(_connectionString);
            conn.Open();

            var cmd = new OracleCommand("SELECT * FROM product WHERE product_id = :ProductId", conn);
            cmd.Parameters.Add(new OracleParameter("ProductId", id));

            using var reader = cmd.ExecuteReader();
            return reader.Read() ? ReadProduct(reader) : null;
        }

        public override string Add(Product product)
        {
            var inputParams = new Dictionary<string, object>
            {
                { "p_product_name", product.ProductName ?? string.Empty },
                { "p_category", product.ProductCategory ?? string.Empty },
                { "p_price", product.ProductPrice },
                { "p_stock", product.ProductStock },
                { "p_product_type", product.ProductType ?? "product" }
            };

            return ExecuteStoredProcedure("add_product", inputParams, "p_message");
        }

        public override string Update(Product product)
        {
            var inputParams = new Dictionary<string, object>
            {
                { "p_product_id", product.ProductId },
                { "p_product_name", product.ProductName ?? string.Empty },
                { "p_category", product.ProductCategory ?? string.Empty },
                { "p_price", product.ProductPrice },
                { "p_stock", product.ProductStock },
                { "p_product_type", product.ProductType ?? "product" }
            };

            return ExecuteStoredProcedure("update_product", inputParams, "p_message");
        }

        public override string Delete(int id)
        {
            var inputParams = new Dictionary<string, object>
            {
                { "p_product_id", id }
            };

            return ExecuteStoredProcedure("delete_product", inputParams, "p_message");
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
                ProductType = reader.GetString(5) // Ensure column index matches actual DB structure
            };
        }
    }
}
