using System.Text;

namespace AzureSqlTest.ConsoleTests
{
    public class SqlCommandBuilder
    {
        private readonly string _tableName;

        public SqlCommandBuilder(string tableName = "[SalesLT].[Address]") { _tableName = tableName; }

        public string BuildInsertCommand(string[] columnNames)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"INSERT INTO {_tableName} (");
            for (int i = 0; i < columnNames.Length; i++)
            {
                sb.Append($"[{columnNames[i]}]");
                if (i < columnNames.Length - 1)
                    sb.Append(", ");
            }
            sb.Append(") VALUES (");
            for (int i = 0; i < columnNames.Length; i++)
            {
                sb.Append($"@{columnNames[i]}");
                if (i < columnNames.Length - 1)
                    sb.Append(", ");
            }
            sb.Append(")");
            return sb.ToString();
        }

        public string BuildReadCommand(string[] columnNames, string? whereClause = null)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT ");
            for (int i = 0; i < columnNames.Length; i++)
            {
                sb.Append($"[{columnNames[i]}]");
                if (i < columnNames.Length - 1)
                    sb.Append(", ");
            }
            sb.Append($" FROM {_tableName}");

            if (!string.IsNullOrEmpty(whereClause))
            {
                sb.Append($" WHERE {whereClause}");
            }

            return sb.ToString();
        }

        public string BuildUpdateCommand(string[] columnNames)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"UPDATE {_tableName} SET ");
            bool hasSetFirstColumn = false;

            foreach (string columnName in columnNames)
            {
                if (columnName.Equals("AddressID", StringComparison.OrdinalIgnoreCase))
                    continue;

                if (hasSetFirstColumn)
                    sb.Append(", ");
                else
                    hasSetFirstColumn = true;

                sb.Append($"[{columnName}] = @{columnName}");
            }

            sb.Append(" WHERE [AddressID] = @AddressID");

            return sb.ToString();
        }

        public string BuildDeleteCommand(string columnName)
        {
            return $"DELETE FROM {_tableName} WHERE [{columnName}] = @{columnName}";
        }
    }
}