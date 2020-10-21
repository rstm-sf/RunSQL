using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Reactive.Linq;
using RunSQL.Models;

namespace RunSQL.Services
{
    internal class SqliteService : IDataService
    {
        private const string SqliteSequence = "sqlite_sequence";

        public string ConnectionString { get; }

        public SqliteService(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public string GetVersion()
        {
            const string connectionString = "Data Source=:memory:";
            const string commandText = "SELECT SQLITE_VERSION()";

            var version = ExecuteScalar(commandText, connectionString).ToString();

            return version;
        }

        public void CreateDatabase()
        {
            var commandText = File.ReadAllText(Constants.CreateDbSqlPath);
            ExecuteNonQuery(commandText, ConnectionString);
        }

        public IObservable<string> GetTableNames() =>
            Observable.Create<string>(
                async obs =>
                {
                    using var reader = ExecuteReader(
                        @"SELECT name FROM sqlite_master WHERE type='table'",
                        ConnectionString);
                    while (await reader.ReadAsync())
                    {
                        var name = reader.GetString(0);
                        if (!name.Equals(SqliteSequence, StringComparison.OrdinalIgnoreCase))
                            obs.OnNext(name);
                    }
                });

        public Table GetResult(string commandText)
        {
            using var reader = ExecuteReader(commandText, ConnectionString);

            var headers = new List<string>(reader.FieldCount);
            for (var i = 0; i < reader.FieldCount; ++i)
                headers.Add(reader.GetName(i));

            var rows = new List<TableRow>();
            while (reader.Read())
            {
                var fields = new List<object>(reader.FieldCount);
                for (var i = 0; i < reader.FieldCount; ++i)
                    fields.Add(reader.GetValue(i));
                rows.Add(new TableRow
                {
                    Fields = fields,
                });
            }

            return new Table
            {
                Headers = headers,
                Rows = rows,
            };
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
