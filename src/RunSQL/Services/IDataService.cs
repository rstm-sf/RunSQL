using System;
using RunSQL.Models;

namespace RunSQL.Services
{
    internal interface IDataService
    {
        string ConnectionString { get; }

        IObservable<string> GetTableNames();

        Table GetResult(string commandText);
    }
}
