using System.IO;

namespace RunSQL
{
    internal static class Constants
    {
        public static readonly string DbFileName = "Chinook_Sqlite_AutoIncrementPKs.sqlite";

        public static readonly string CreateDbSqlFileName = "Chinook_Sqlite_AutoIncrementPKs.sql";

        public static readonly string DbDirectory = Path.Combine("Database");

        public static readonly string DbPath = Path.Combine(DbDirectory, DbFileName);

        public static readonly string CreateDbSqlPath = Path.Combine(DbDirectory, CreateDbSqlFileName);
    }
}
