using System.ComponentModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Markup.Xaml;
using RunSQL.Models;
using RunSQL.Services;
using RunSQL.ViewModels;

namespace RunSQL.Views
{
    public class MainWindow : Window
    {
        private readonly DataGrid _dataGrid;

        public MainWindow()
        {
            InitializeComponent();

            _dataGrid = this.FindControl<DataGrid>("dataGrid");

            InitializeViewModel();

#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void InitializeViewModel()
        {
            var connectionString = $"URI=file:{Constants.DbPath}";
            var viewModel = new MainWindowViewModel(new SqliteService(connectionString));
            DataContext = viewModel;
            viewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(MainWindowViewModel.Table))
                CreateDataGridColumnsBinding(((MainWindowViewModel)DataContext).Table);
        }

        private void CreateDataGridColumnsBinding(Table table)
        {
            _dataGrid.BeginEdit();

            _dataGrid.Columns.Clear();
            for (var i = 0; i < table.Headers.Count; ++i)
                _dataGrid.Columns.Add(new DataGridTextColumn
                {
                    Header = table.Headers[i],
                    Binding = new Binding($"{nameof(TableRow.Fields)}[{i}]", BindingMode.OneWay),
                });

            _dataGrid.CommitEdit();
        }
    }
}
