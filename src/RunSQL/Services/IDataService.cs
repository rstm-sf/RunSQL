using System.Collections.Generic;
using RunSQL.Models;

namespace RunSQL.Services
{
    internal interface IDataService
    {
        public string ConnectionString { get; }

        IEnumerable<string> GetTableNames();

        Table GetResult(string commandText);
    }
}
