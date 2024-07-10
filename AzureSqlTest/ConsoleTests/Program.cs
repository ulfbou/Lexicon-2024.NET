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
                string your_password = "Asdf1234!";
                builder.ConnectionString = $"Server=tcp:ado-crud-2110-02501791.database.windows.net,1433;Initial Catalog=ADO.CRUD;Persist Security Info=False;User ID=ado-crud-admin;Password={your_password};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    await connection.OpenAsync();

                    ReadDatabaseMetadata(connection);
                    //ReadAddressSchema(connection);

                    AddressCRUD addressCRUD = new AddressCRUD(connection);

                    await Utility.RunAsync(addressCRUD);
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        // Generate code to read the tables and columns in the database and display on the console.
        private static void ReadDatabaseMetadata(SqlConnection connection)
        {
            DataTable schema = connection.GetSchema("Tables");
            foreach (DataRow row in schema.Rows)
            {
                string tableCatalog = row["TABLE_CATALOG"].ToString();
                string tableSchema = row["TABLE_SCHEMA"].ToString();
                string tableName = row["TABLE_NAME"].ToString();
                Console.WriteLine($"Table: {tableName}");

                //DataTable columnsSchema = connection.GetSchema("Columns", new[] { null, null, tableName });
                //foreach (DataRow columnRow in columnsSchema.Rows)
                //{
                //    string columnName = columnRow["COLUMN_NAME"].ToString();
                //    string dataType = columnRow["DATA_TYPE"].ToString();
                //    Console.WriteLine($"\tColumn: {columnName}, Type: {dataType}");
                //}
            }

            // Generate tables for 
        }

        private static Dictionary<string, string> ReadAddressSchema(SqlConnection connection)
        {
            Dictionary<string, string> addressSchema = new Dictionary<string, string>();
            DataTable columnsSchema = connection.GetSchema("Columns", new[] { null, null, "SalesLT.Address" });
            foreach (DataRow columnRow in columnsSchema.Rows)
            {
                string columnName = columnRow["COLUMN_NAME"].ToString();
                string dataType = columnRow["DATA_TYPE"].ToString();
                addressSchema.Add(columnName, dataType);
            }

            return addressSchema;
        }

    }
}
