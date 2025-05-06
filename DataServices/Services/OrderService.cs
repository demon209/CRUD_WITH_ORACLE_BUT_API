using MVC.Models;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;

namespace MVC.Services
{
    public class OrderService : IOrderService
    {
        private readonly string _connectionString = Environment.GetEnvironmentVariable("ORACLE_CONN_STRING");

        public List<Order> GetAllOrders()
        {
            List<Order> orders = new();

            using var conn = new OracleConnection(_connectionString);
            conn.Open();

            string query = "SELECT order_id, customer_id, order_date, total_amount FROM orders ORDER BY order_id ASC";

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
            List<Order> orders = new();

            using var conn = new OracleConnection(_connectionString);
            conn.Open();

            string query = @"SELECT order_id, customer_id, order_date, total_amount 
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

        public Order GetOrderById(int id)
        {
            using var conn = new OracleConnection(_connectionString);
            conn.Open();

            string query = "SELECT order_id, customer_id, order_date, total_amount FROM orders WHERE order_id = :id";

            using var cmd = new OracleCommand(query, conn);
            cmd.Parameters.Add(new OracleParameter("id", id));

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new Order
                {
                    OrderId = reader.GetInt32(0),
                    CustomerId = reader.GetInt32(1),
                    OrderDate = reader.GetDateTime(2),
                    TotalAmount = reader.GetDecimal(3)
                };
            }

            return reader.Read() ? ReadOrder(reader) : null;
        }

        public string AddOrder(Order order)
        {
            using var conn = new OracleConnection(_connectionString);
            conn.Open();

            using var cmd = new OracleCommand("add_order", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.Add("p_customer_id", OracleDbType.Int32).Value = order.CustomerId;
            cmd.Parameters.Add("p_order_date", OracleDbType.Date).Value = order.OrderDate;
            cmd.Parameters.Add("p_total_amount", OracleDbType.Decimal).Value = order.TotalAmount;

            var messageParam = new OracleParameter("p_message", OracleDbType.Varchar2, 4000)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(messageParam);

            cmd.ExecuteNonQuery();

            return messageParam.Value.ToString();
        }

        public string UpdateOrder(Order order)
        {
            using var conn = new OracleConnection(_connectionString);
            conn.Open();

            using var cmd = new OracleCommand("update_order", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.Add("p_order_id", OracleDbType.Int32).Value = order.OrderId;
            cmd.Parameters.Add("p_customer_id", OracleDbType.Int32).Value = order.CustomerId;
            cmd.Parameters.Add("p_order_date", OracleDbType.Date).Value = order.OrderDate;
            cmd.Parameters.Add("p_total_amount", OracleDbType.Decimal).Value = order.TotalAmount;

            var messageParam = new OracleParameter("p_message", OracleDbType.Varchar2, 4000)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(messageParam);

            cmd.ExecuteNonQuery();

            return messageParam.Value.ToString();
        }

        public string DeleteOrder(int orderId)
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

            return cmd.Parameters["p_message"].Value.ToString();
        }
        private Order ReadOrder(OracleDataReader reader)
        {
            return new Order
            {
                OrderId = reader.GetInt32(0),
                CustomerId = reader.GetInt32(1),
                OrderDate = reader.GetDateTime(2),
                TotalAmount = reader.GetDecimal(3)
            };
        }
    }
}
