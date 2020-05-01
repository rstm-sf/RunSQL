using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;

namespace RunSQL.Services
{
    internal class SqliteService
    {
        public string GetVersion()
        {
            const string connectionString = "Data Source=:memory:";
            const string commandText = "SELECT SQLITE_VERSION()";

            var version = ExecuteScalar(commandText, connectionString).ToString();

            return version;
        }

        public void CreateDatabase(string connectionString)
        {
            var commandText = File.ReadAllText(Constants.CreateDbSqlPath);
            ExecuteNonQuery(commandText, connectionString);
        }

        public IEnumerable<string> GetTableNames(string connectionString)
        {
            const string commandText = @"SELECT name FROM sqlite_master WHERE type='table'";
            using var reader = ExecuteReader(commandText, connectionString);
            while (reader.Read())
                yield return reader.GetString(0);
        }

        private static SQLiteConnection CreateConnection(string connectionString) =>
            new SQLiteConnection(connectionString);

        private static SQLiteConnection CreateAndOpenConnection(string connectionString)
        {
            var connection = CreateConnection(connectionString);
            connection.Open();
            return connection;
        }

        private static SQLiteCommand CreateCommand(string commandText, SQLiteConnection connection) =>
            new SQLiteCommand(commandText, connection);

        private static SQLiteCommand CreateCommandAndOpenConnection(string commandText, string connectionString) =>
            new SQLiteCommand(commandText, CreateAndOpenConnection(connectionString));

        private static SQLiteDataReader ExecuteReader(string commandText, string connectionString) =>
            CreateCommandAndOpenConnection(commandText, connectionString)
                .ExecuteReader(CommandBehavior.CloseConnection);

        private static object ExecuteScalar(string commandText, string connectionString) =>
            CreateCommandAndOpenConnection(commandText, connectionString)
                .ExecuteScalar(CommandBehavior.CloseConnection);

        private static int ExecuteNonQuery(string commandText, string connectionString) =>
            CreateCommandAndOpenConnection(commandText, connectionString)
                .ExecuteNonQuery(CommandBehavior.CloseConnection);
    }
}
