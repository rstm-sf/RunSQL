using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using ReactiveUI;
using RunSQL.Models;
using RunSQL.Services;

namespace RunSQL.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly IDataService _dataService;

        private string _commandText;

        private Table _table;

        private string _errorMessage;

        public IReadOnlyList<string> TableNames { get; }

        public string CommandText
        {
            get => _commandText;
            set => this.RaiseAndSetIfChanged(ref _commandText, value);
        }

        public Table Table
        {
            get => _table;
            set => this.RaiseAndSetIfChanged(ref _table, value);
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            private set => this.RaiseAndSetIfChanged(ref _errorMessage, value);
        }

        public bool IsVisibleDataGrid => string.IsNullOrEmpty(ErrorMessage);

        public ReactiveCommand<Unit, Unit> Run { get; }

        public ReactiveCommand<string, Unit> TableNameClick { get; }

        public MainWindowViewModel(IDataService dataService)
        {
            _dataService = dataService;

            TableNames = GetTableNames();

            _table = Table.Empty;
            _commandText = string.Empty;
            _errorMessage = " ";

            this.WhenAnyValue(x => x.ErrorMessage)
                .Subscribe(_ => this.RaisePropertyChanged(nameof(IsVisibleDataGrid)));

            Run = ReactiveCommand.Create(
                () =>
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
                this.WhenAnyValue(x => x.CommandText, x => !string.IsNullOrWhiteSpace(x)));

            TableNameClick = ReactiveCommand.Create((string parameter) =>
            {
                CommandText = $"SELECT * FROM {parameter};";
            });
        }

        private List<string> GetTableNames()
        {
            try
            {
                return _dataService.GetTableNames()
                    .ToEnumerable()
                    .ToList();
            }
            catch (Exception ex)
            {
                return new List<string>();
            }
        }

        private Table GetCommandResult() => _dataService.GetResult(CommandText);
    }
}
