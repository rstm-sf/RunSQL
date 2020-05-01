using System.Collections.ObjectModel;
using RunSQL.Models;
using RunSQL.Services;

namespace RunSQL.ViewModels
{
    internal class MainWindowViewModel : Observable
    {
        private static readonly SqliteService SqliteService = new SqliteService();

        public MainWindowViewModel()
        {
            var connectionString = $"URI=file:{Constants.DbPath}";
            var names = SqliteService.GetTableNames(connectionString);
            Items = new ObservableCollection<string>(names);
        }

        public ObservableCollection<string> Items { get; }
    }
}
