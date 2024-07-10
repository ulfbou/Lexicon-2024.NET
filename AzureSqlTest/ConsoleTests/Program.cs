using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Net;

namespace AzureSqlTest.ConsoleTests
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                string your_password = "your password";
                builder.ConnectionString = $"Server=tcp:your-database-server.database.windows.net,something;Initial Catalog=Your.Initial.Catalog;Persist Security Info=False;User ID=your-user-id;Password={your_password};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    await connection.OpenAsync();
                    await Utility.RunAsync(new AddressCRUD(connection));
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}