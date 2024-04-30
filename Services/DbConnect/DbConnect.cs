using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace TodoList.Services.DbConnect
{
    class DbConnect
    {
        SqlConnection connection;
        public DbConnect() {
            string connString = "Data Source = DESKTOP-T97GK5V;Initial Catalog=ToDoList;Integrated Security=True;";
            this.connection = new SqlConnection(connString);
        }

        public void Open()
        {
            connection.Open();
        }

        public void Close()
        {
            connection.Close();
        }

        public SqlDataReader GetDataByQuery(string query)
        {
            SqlCommand cmd = new SqlCommand(query, connection);
            SqlDataReader reader = cmd.ExecuteReader();
            
            return reader;
        }

        public int GetLastIdByTable(string tableName)
        {
            connection.Open();
            string query = "SELECT TOP 1 Id FROM [" + tableName + "] ORDER BY Id DESC";

            SqlCommand cmd = new SqlCommand(query, connection);
            object result = cmd.ExecuteScalar();

            // when table is clear tretment
            int lastId = 0;
            if (result != null)
            {
                lastId = (int)result;
            }
            
            connection.Close();

            return lastId;
        }

        public void InsertIntoTable(string tableName, string value1)
        {
            connection.Open();
            string query = "INSERT INTO [" + tableName + "] (value1) VALUES (@Value1)";

            SqlCommand sql = new SqlCommand(query, connection);
            sql.Parameters.AddWithValue("@Value1", value1);
            sql.ExecuteNonQuery();
            connection.Close();
        }

        public int GetIdByTableAndName(string tableName, string colName, string data)
        {
            connection.Open();
            string query = "SELECT Id FROM [" + tableName + "] WHERE " + colName + " = " + data;

            SqlCommand cmd = new SqlCommand(query, connection);
            int id = (int)cmd.ExecuteScalar();
            connection.Close();

            return id;

        }

        public SqlConnection GetConnection()
        {
            return connection;
        }
    }
}
