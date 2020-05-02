using System.Collections.Generic;
using System.Linq;
using RunSQL.Models;
using RunSQL.Services;

namespace RunSQL.ViewModels
{
    internal class MainWindowViewModel : Observable
    {
        private static readonly SqliteService SqliteService = new SqliteService();

        private string _commandText;

        private Table _table;

        public IReadOnlyList<string> TableNames { get; }

        public string CommandText
        {
            get => _commandText;
            set
            {
                _commandText = value;
                NotifyPropertyChanged();
            }
        }

        public Table Table
        {
            get => _table;
            set
            {
                _table = value;
                NotifyPropertyChanged();
            }
        }

        public MainWindowViewModel()
        {
            var connectionString = $"URI=file:{Constants.DbPath}";
            TableNames = SqliteService.GetTableNames(connectionString)
                .ToList();
            CommandText = @"SELECT * FROM Album";
            Table = SqliteService.GetResult(CommandText, connectionString);
        }
    }
}
