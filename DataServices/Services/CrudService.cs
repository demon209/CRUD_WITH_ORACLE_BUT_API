using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System.Data.SqlClient;

namespace MVC.Services
{
    public class CrudService<T> : ICrudService<T> where T : class, new()
    {
        protected readonly string _connectionString;
        protected readonly string _providerName;

        public CrudService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _providerName = configuration.GetSection("ConnectionStrings:ProviderName").Value;
        }

        protected DbConnection CreateConnection()
        {
            return _providerName switch
            {
                "Oracle.ManagedDataAccess.Client" => new OracleConnection(_connectionString),
                "System.Data.SqlClient" => new SqlConnection(_connectionString),
                _ => throw new NotSupportedException("Provider not supported"),
            };
        }

        protected DbCommand CreateCommand(string sql, DbConnection conn)
        {
            return _providerName switch
            {
                "Oracle.ManagedDataAccess.Client" => new OracleCommand(sql, (OracleConnection)conn),
                "System.Data.SqlClient" => new SqlCommand(sql, (SqlConnection)conn),
                _ => throw new NotSupportedException("Provider not supported"),
            };
        }

        /// <summary>
        /// Hàm dùng cho Oracle để thực thi stored procedure có output message
        /// </summary>
        protected string ExecuteStoredProcedure(string procedureName, Dictionary<string, object> inputParams, string outputParamName)
        {
            if (_providerName != "Oracle.ManagedDataAccess.Client")
                throw new NotSupportedException("ExecuteStoredProcedure chỉ hỗ trợ Oracle");

            using var conn = new OracleConnection(_connectionString);
            conn.Open();

            using var cmd = new OracleCommand(procedureName, conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            // Add input parameters
            foreach (var kv in inputParams)
            {
                cmd.Parameters.Add(kv.Key, kv.Value ?? DBNull.Value);
            }

            // Add output parameter
            var output = new OracleParameter(outputParamName, OracleDbType.Varchar2, 4000)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(output);

            cmd.ExecuteNonQuery();
            return output.Value?.ToString();
        }

        public virtual List<T> GetAll()
        {
            var items = new List<T>();
            var tableName = typeof(T).Name;

            using var conn = CreateConnection();
            conn.Open();

            using var cmd = CreateCommand($"SELECT * FROM {tableName} ORDER BY 1 ASC", conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var item = ReadEntity<T>(reader);
                items.Add(item);
            }

            return items;
        }

        public virtual T GetById(int id)
        {
            var tableName = typeof(T).Name;

            using var conn = CreateConnection();
            conn.Open();

            using var cmd = CreateCommand($"SELECT * FROM {tableName} WHERE id = @id", conn);
            var param = cmd.CreateParameter();
            param.ParameterName = "@id";
            param.Value = id;
            cmd.Parameters.Add(param);

            using var reader = cmd.ExecuteReader();
            return reader.Read() ? ReadEntity<T>(reader) : null;
        }

        public virtual string Add(T entity)
        {
            var tableName = typeof(T).Name;
            var properties = typeof(T).GetProperties();
            var columns = string.Join(", ", properties.Select(p => p.Name));
            var values = string.Join(", ", properties.Select(p => $"@{p.Name}"));

            using var conn = CreateConnection();
            conn.Open();

            using var cmd = CreateCommand($"INSERT INTO {tableName} ({columns}) VALUES ({values})", conn);

            foreach (var prop in properties)
            {
                var param = cmd.CreateParameter();
                param.ParameterName = $"@{prop.Name}";
                param.Value = prop.GetValue(entity) ?? DBNull.Value;
                cmd.Parameters.Add(param);
            }

            try
            {
                return cmd.ExecuteNonQuery() > 0 ? "Insert successful" : "Insert failed";
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }

        public virtual string Update(T entity)
        {
            var tableName = typeof(T).Name;
            var properties = typeof(T).GetProperties();
            var setClause = string.Join(", ", properties.Select(p => $"{p.Name} = @{p.Name}"));

            using var conn = CreateConnection();
            conn.Open();

            using var cmd = CreateCommand($"UPDATE {tableName} SET {setClause} WHERE id = @id", conn);

            foreach (var prop in properties)
            {
                var param = cmd.CreateParameter();
                param.ParameterName = $"@{prop.Name}";
                param.Value = prop.GetValue(entity) ?? DBNull.Value;
                cmd.Parameters.Add(param);
            }

            var idProp = properties.FirstOrDefault(p => p.Name.Equals("id", StringComparison.OrdinalIgnoreCase));
            if (idProp != null)
            {
                var idParam = cmd.CreateParameter();
                idParam.ParameterName = "@id";
                idParam.Value = idProp.GetValue(entity);
                cmd.Parameters.Add(idParam);
            }

            try
            {
                return cmd.ExecuteNonQuery() > 0 ? "Update successful" : "Update failed";
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }

        public virtual string Delete(int id)
        {
            var tableName = typeof(T).Name;

            using var conn = CreateConnection();
            conn.Open();

            using var cmd = CreateCommand($"DELETE FROM {tableName} WHERE id = @id", conn);
            var param = cmd.CreateParameter();
            param.ParameterName = "@id";
            param.Value = id;
            cmd.Parameters.Add(param);

            try
            {
                return cmd.ExecuteNonQuery() > 0 ? "Delete successful" : "Delete failed";
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }

        protected T ReadEntity<T>(DbDataReader reader) where T : new()
        {
            var entity = new T();
            var properties = typeof(T).GetProperties();

            for (int i = 0; i < reader.FieldCount; i++)
            {
                var col = reader.GetName(i);
                var prop = properties.FirstOrDefault(p => p.Name.Equals(col, StringComparison.OrdinalIgnoreCase));
                if (prop != null && prop.CanWrite)
                {
                    var value = reader.IsDBNull(i) ? null : reader.GetValue(i);
                    prop.SetValue(entity, value);
                }
            }

            return entity;
        }
    }
}
