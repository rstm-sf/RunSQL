using System.Reactive;
using System.Reactive.Disposables;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using RunSQL.Models;
using RunSQL.ViewModels;

namespace RunSQL.Views
{
    public class MainWindow : ReactiveWindow<MainWindowViewModel>
    {
        private DataGrid DataGrid => this.FindControl<DataGrid>("dataGrid");

        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            this.WhenActivated(disposables =>
            {
                this.WhenAnyValue(v => v.ViewModel.Table)
                    .Subscribe(Observer.Create<Table>(table =>
                    {
                        DataGrid.Columns.Clear();
                        for (var i = 0; i < table.Headers.Count; ++i)
                            DataGrid.Columns.Add(new DataGridTextColumn
                            {
                                Header = table.Headers[i],
                                Binding = new Binding($"{nameof(TableRow.Fields)}[{i}]", BindingMode.OneWay),
                            });
                    }))
                    .DisposeWith(disposables);
            });
            AvaloniaXamlLoader.Load(this);
        }
    }
}
