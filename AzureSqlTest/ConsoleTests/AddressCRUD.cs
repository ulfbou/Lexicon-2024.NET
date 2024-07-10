using Microsoft.Data.SqlClient;

namespace AzureSqlTest.ConsoleTests
{
    public class AddressCRUD(SqlConnection connection)
    {
        private SqlConnection _connection = connection ?? throw new ArgumentNullException(nameof(connection));
        SqlCommandBuilder _sqlCommandBuilder = new SqlCommandBuilder();

        public async Task<bool> TryCreateAsync(string addressLine1, string city, string stateProvince, string countryRegion, string postalCode, string addressLine2 = "")
        {
            string createSql = _sqlCommandBuilder.BuildInsertCommand(new string[] { "AddressLine1", "AddressLine2", "City", "StateProvince", "CountryRegion", "PostalCode" });
            try
            {
                await Console.Out.WriteLineAsync(createSql);
                using (SqlCommand command = new SqlCommand(createSql, _connection))
                {
                    command.Parameters.AddWithValue("@AddressLine1", addressLine1);
                    command.Parameters.AddWithValue("@AddressLine2", addressLine2);
                    command.Parameters.AddWithValue("@City", city);
                    command.Parameters.AddWithValue("@StateProvince", stateProvince);
                    command.Parameters.AddWithValue("@CountryRegion", countryRegion);
                    command.Parameters.AddWithValue("@PostalCode", postalCode);
                    await command.ExecuteNonQueryAsync();
                }
                return true;
            }
            catch (SqlException e)
            {
                await Console.Out.WriteLineAsync(e.Message);
                return false;
            }
        }

        public async Task<bool> TryReadAsync(string parameterName, string parameterValue)
        {
            string whereClause = $"[{parameterName}] = @{parameterName}";
            string readSql = _sqlCommandBuilder.BuildReadCommand(new string[] { "AddressLine1", "AddressLine2", "City", "StateProvince", "CountryRegion", "PostalCode" }, whereClause);
            try
            {
                await Console.Out.WriteLineAsync(readSql);
                using (SqlCommand command = new SqlCommand(readSql, _connection))
                {
                    command.Parameters.AddWithValue($"@{parameterName}", parameterValue);

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var addressLine1 = reader.GetString(0);
                            var addressLine2 = reader.IsDBNull(1) ? string.Empty : reader.GetString(1);
                            var city = reader.GetString(2);
                            var stateProvince = reader.GetString(3);
                            var countryRegion = reader.GetString(4);
                            var postalCode = reader.GetString(5);
                            await Console.Out.WriteLineAsync($"{addressLine1}, {addressLine2}, {city}, {stateProvince}, {countryRegion}, {postalCode}");
                        }
                    }
                }
                return true;
            }
            catch (SqlException e)
            {
                await Console.Out.WriteLineAsync(e.Message);
                return false;
            }
        }

        public async Task<bool> TryUpdateAsync(int addressId, string? addressLine1, string? city, string? stateProvince, string? countryRegion, string? postalCode, string? addressLine2 = "")
        {
            List<string> parameters = new List<string>();

            if (!string.IsNullOrEmpty(addressLine1)) parameters.Add("AddressLine1");
            if (!string.IsNullOrEmpty(addressLine2)) parameters.Add("AddressLine2");
            if (!string.IsNullOrEmpty(city)) parameters.Add("City");
            if (!string.IsNullOrEmpty(stateProvince)) parameters.Add("StateProvince");
            if (!string.IsNullOrEmpty(countryRegion)) parameters.Add("CountryRegion");
            if (!string.IsNullOrEmpty(postalCode)) parameters.Add("PostalCode");

            string updateSql = _sqlCommandBuilder.BuildUpdateCommand(parameters.ToArray());
            await Console.Out.WriteLineAsync(updateSql);
            try
            {
                using (SqlCommand command = new SqlCommand(updateSql, _connection))
                {
                    if (!string.IsNullOrEmpty(addressLine1)) command.Parameters.AddWithValue("@AddressLine1", addressLine1);
                    if (!string.IsNullOrEmpty(addressLine2)) command.Parameters.AddWithValue("@AddressLine2", addressLine2);
                    if (!string.IsNullOrEmpty(city)) command.Parameters.AddWithValue("@City", city);
                    if (!string.IsNullOrEmpty(stateProvince)) command.Parameters.AddWithValue("@StateProvince", stateProvince);
                    if (!string.IsNullOrEmpty(countryRegion)) command.Parameters.AddWithValue("@CountryRegion", countryRegion);
                    if (!string.IsNullOrEmpty(postalCode)) command.Parameters.AddWithValue("@PostalCode", postalCode);

                    command.Parameters.AddWithValue("@AddressID", addressId);

                    await command.ExecuteNonQueryAsync();
                }
                return true;
            }
            catch (SqlException e)
            {
                await Console.Out.WriteLineAsync(e.Message);
                return false;
            }
        }

        public async Task<bool> TryDeleteAsync(int addressId)
        {
            string deleteSql = _sqlCommandBuilder.BuildDeleteCommand("AddressLine1");
            try
            {
                using (SqlCommand command = new SqlCommand(deleteSql, _connection))
                {
                    command.Parameters.AddWithValue("@AddressId", addressId);
                    await command.ExecuteNonQueryAsync();
                }
                return true;
            }
            catch (SqlException e)
            {
                await Console.Out.WriteLineAsync(e.Message);
                return false;
            }
        }
    }
}