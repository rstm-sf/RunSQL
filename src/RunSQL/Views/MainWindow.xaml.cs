using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using AvaloniaEdit;
using ReactiveUI;
using RunSQL.Models;
using RunSQL.ViewModels;

namespace RunSQL.Views
{
    public class MainWindow : ReactiveWindow<MainWindowViewModel>
    {
        private DataGrid DataGrid => this.FindControl<DataGrid>("DataGrid");

        private TextEditor TextEditor => this.FindControl<TextEditor>("TextEditor");

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

                this.Bind(ViewModel, vm => vm.CommandText, v => v.TextEditor.Text)
                    .DisposeWith(disposables);

                Observable
                    .FromEventPattern(this.TextEditor, nameof(TextEditor.TextChanged))
                    .Subscribe(Observer.Create<EventPattern<object>>(_ =>
                    {
                        ViewModel.CommandText = TextEditor.Text;
                    }))
                    .DisposeWith(disposables);
            });
            AvaloniaXamlLoader.Load(this);
        }
    }
}
