using System.Collections.Generic;
using RunSQL.Models;

namespace RunSQL.Services
{
    internal interface IDataService
    {
        IEnumerable<string> GetTableNames(string connectionString);

        Table GetResult(string commandText, string connectionString);
    }
}
