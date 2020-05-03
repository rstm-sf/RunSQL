using System;
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

        private string _errorMessage;

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

        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                NotifyPropertyChanged();
            }
        }

        public bool IsVisibleDataGrid => string.IsNullOrEmpty(ErrorMessage);

        public DelegateCommand Run { get; }

        public DelegateCommand TableNameClick { get; }

        public MainWindowViewModel()
        {
            TableNames = GetTableNames();

            Run = new DelegateCommand(
                parameter =>
                {
                    try
                    {
                        Table = GetCommandResult();
                        ErrorMessage = string.Empty;
                    }
                    catch (Exception e)
                    {
                        Table = Table.Empty;
                        ErrorMessage = e.Message;
                    }
                },
                parameter => !string.IsNullOrWhiteSpace(CommandText));

            TableNameClick = new DelegateCommand(parameter =>
            {
                CommandText = $"SELECT * FROM {parameter};";
            });

            ErrorMessage = " "; // for unvisible datagrid when start

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
                switch (args.PropertyName)
                {
                    case nameof(CommandText):
                        Run.RaiseCanExecuteChanged();
                        break;
                    case nameof(ErrorMessage):
                        NotifyPropertyChanged(nameof(IsVisibleDataGrid));
                        break;
                }
            };
    }
}
