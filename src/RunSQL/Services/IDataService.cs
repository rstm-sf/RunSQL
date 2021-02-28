using System;
using RunSQL.Models;

namespace RunSQL.Services
{
    public interface IDataService
    {
        string ConnectionString { get; }

        IObservable<string> GetTableNames();

        Table GetResult(string commandText);
    }
}
