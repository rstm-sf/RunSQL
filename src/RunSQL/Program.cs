using System;
using RunSQL.Services;

namespace RunSQL
{
    class Program
    {
        private static readonly SqliteService SqliteService = new SqliteService();

        public static void Main()
        {
            Console.WriteLine($"SQLite version: {SqliteService.GetVersion()}");

            var connectionString = $"URI=file:{Constants.DbPath}";
            var names = SqliteService.GetTableNames(connectionString);
            foreach (var name in names)
                Console.WriteLine(name);
        }
    }
}
