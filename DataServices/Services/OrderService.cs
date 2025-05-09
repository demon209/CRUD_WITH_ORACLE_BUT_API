using MVC.Models;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Extensions.Configuration;

namespace MVC.Services
{
    public class OrderService : CrudService<Order>, IOrderService
    {
        public OrderService(IConfiguration configuration) : base(configuration) { }

        public override List<Order> GetAll()
        {
            var orders = new List<Order>();
            using var conn = new OracleConnection(_connectionString);
            conn.Open();

            string query = @"SELECT order_id, customer_id, pet_id, product_id, quantity, order_date, total_amount 
                             FROM orders 
                             ORDER BY order_id ASC";

            using var cmd = new OracleCommand(query, conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                orders.Add(ReadOrder(reader));
            }

            return orders;
        }

        public List<Order> SearchOrders(string keyword)
        {
            var orders = new List<Order>();
            using var conn = new OracleConnection(_connectionString);
            conn.Open();

            string query = @"SELECT order_id, customer_id, pet_id, product_id, quantity, order_date, total_amount 
                             FROM orders 
                             WHERE TO_CHAR(order_date, 'YYYY-MM-DD') LIKE :keyword 
                             ORDER BY order_id ASC";

            using var cmd = new OracleCommand(query, conn);
            cmd.Parameters.Add(new OracleParameter("keyword", $"%{keyword}%"));

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                orders.Add(ReadOrder(reader));
            }

            return orders;
        }

        public override Order GetById(int id)
        {
            using var conn = new OracleConnection(_connectionString);
            conn.Open();

            string query = @"SELECT order_id, customer_id, pet_id, product_id, quantity, order_date, total_amount 
                             FROM orders 
                             WHERE order_id = :id";

            using var cmd = new OracleCommand(query, conn);
            cmd.Parameters.Add(new OracleParameter("id", id));

            using var reader = cmd.ExecuteReader();
            return reader.Read() ? ReadOrder(reader) : null;
        }

        public override string Add(Order order)
        {
            using var conn = new OracleConnection(_connectionString);
            conn.Open();

            using var cmd = new OracleCommand("add_order", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.Add("p_customer_id", OracleDbType.Int32).Value = order.CustomerId;
            cmd.Parameters.Add("p_pet_id", OracleDbType.Int32).Value = order.PetId;
            cmd.Parameters.Add("p_product_id", OracleDbType.Int32).Value = order.ProductId;
            cmd.Parameters.Add("p_quantity", OracleDbType.Int32).Value = order.ProductQuantity;
            cmd.Parameters.Add("p_order_date", OracleDbType.Date).Value = order.OrderDate;

            var totalAmountParam = new OracleParameter("p_total_amount", OracleDbType.Decimal)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(totalAmountParam);

            var messageParam = new OracleParameter("p_message", OracleDbType.Varchar2, 4000)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(messageParam);

            cmd.ExecuteNonQuery();

            order.TotalAmount = GetDecimalFromOracleParam(totalAmountParam);

            return messageParam.Value?.ToString() ?? "Không có phản hồi từ thủ tục.";
        }

        public override string Update(Order order)
        {
            using var conn = new OracleConnection(_connectionString);
            conn.Open();

            using var cmd = new OracleCommand("update_order", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.Add("p_order_id", OracleDbType.Int32).Value = order.OrderId;
            cmd.Parameters.Add("p_customer_id", OracleDbType.Int32).Value = order.CustomerId;
            cmd.Parameters.Add("p_pet_id", OracleDbType.Int32).Value = order.PetId;
            cmd.Parameters.Add("p_product_id", OracleDbType.Int32).Value = order.ProductId;
            cmd.Parameters.Add("p_quantity", OracleDbType.Int32).Value = order.ProductQuantity;
            cmd.Parameters.Add("p_order_date", OracleDbType.Date).Value = order.OrderDate;

            var totalAmountParam = new OracleParameter("p_total_amount", OracleDbType.Decimal)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(totalAmountParam);

            var messageParam = new OracleParameter("p_message", OracleDbType.Varchar2, 4000)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(messageParam);

            cmd.ExecuteNonQuery();

            order.TotalAmount = GetDecimalFromOracleParam(totalAmountParam);

            return messageParam.Value?.ToString() ?? "Không có phản hồi từ thủ tục.";
        }

        public override string Delete(int orderId)
        {
            using var conn = new OracleConnection(_connectionString);
            conn.Open();

            using var cmd = new OracleCommand("delete_order", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.Add("p_order_id", OracleDbType.Int32).Value = orderId;
            cmd.Parameters.Add("p_message", OracleDbType.Varchar2, 4000).Direction = ParameterDirection.Output;

            cmd.ExecuteNonQuery();
            return cmd.Parameters["p_message"].Value?.ToString() ?? "Không có phản hồi từ thủ tục.";
        }

        private Order ReadOrder(OracleDataReader reader)
        {
            return new Order
            {
                OrderId = reader.GetInt32(0),
                CustomerId = reader.GetInt32(1),
                PetId = reader.IsDBNull(2) ? (int?)null : reader.GetInt32(2),
                ProductId = reader.IsDBNull(3) ? (int?)null : reader.GetInt32(3),
                ProductQuantity = reader.IsDBNull(4) ? (int?)null : reader.GetInt32(4),
                OrderDate = reader.GetDateTime(5),
                TotalAmount = reader.IsDBNull(6) ? 0 : reader.GetDecimal(6)
            };
        }

        private decimal GetDecimalFromOracleParam(OracleParameter param)
        {
            return param?.Value is OracleDecimal dec && !dec.IsNull ? dec.Value : 0;
        }
    }
}
