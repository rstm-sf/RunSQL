using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using RunSQL.Commands;
using RunSQL.Models;
using RunSQL.Services;

namespace RunSQL.ViewModels
{
    internal class MainWindowViewModel : Observable
    {
        private readonly string _connectionString = $"URI=file:{Constants.DbPath}";

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

        public DelegateCommand Run { get; }

        public MainWindowViewModel()
        {
            TableNames = GetTableNames();

            Run = new DelegateCommand(
                parameter => Table = GetCommandResult(),
                parameter => !string.IsNullOrWhiteSpace(CommandText));

            PropertyChanged += OnPropertyChanged();
        }

        private List<string> GetTableNames() =>
            SqliteService.GetTableNames(_connectionString)
                .ToList();

        private Table GetCommandResult() =>
            SqliteService.GetResult(CommandText, _connectionString);

        private PropertyChangedEventHandler OnPropertyChanged() =>
            (sender, args) =>
            {
                if (args.PropertyName == nameof(CommandText))
                    Run.RaiseCanExecuteChanged();
            };
    }
}
