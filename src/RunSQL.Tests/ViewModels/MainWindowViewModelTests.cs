using System;
using System.Collections.Generic;
using Moq;
using RunSQL.Models;
using RunSQL.Services;
using RunSQL.ViewModels;
using Xunit;

namespace RunSQL.Tests.ViewModels
{
    public class MainWindowViewModelTests
    {
        private const string CommandTextResultEmpty = nameof(CommandTextResultEmpty);
        private const string CommandTextResultThrowException = nameof(CommandTextResultThrowException);

        private const string ErrorMessage = nameof(ErrorMessage);

        private static readonly IReadOnlyList<string> TableNames = new List<string>
        {
            "Table 1",
            "Table 2",
            "Table 3",
        };

        private readonly MainWindowViewModel _viewModel;

        public static IEnumerable<object[]> TableNamesTestData()
        {
            foreach (var name in TableNames)
                yield return new object[] { name, };
        }

        public MainWindowViewModelTests()
        {
            var dataServiceMock = CreateDataServiceMock();
            _viewModel = new MainWindowViewModel(dataServiceMock.Object);
        }

        private static Mock<IDataService> CreateDataServiceMock()
        {
            var dataServiceMock = new Mock<IDataService>();
            dataServiceMock.Setup(x => x.GetTableNames())
                .Returns(TableNames);
            dataServiceMock.Setup(x => x.GetResult(CommandTextResultEmpty))
                .Returns(Table.Empty);
            dataServiceMock.Setup(x => x.GetResult(CommandTextResultThrowException))
                .Throws(new Exception(ErrorMessage));
            return dataServiceMock;
        }

        [Fact]
        public void ShouldStartsAppWithTableNamesEqualsSetnames()
        {
            for (var i = 0; i < TableNames.Count; ++i)
                Assert.Equal(TableNames[i], _viewModel.TableNames[i]);
        }

        [Fact]
        public void ShouldStartsAppWithErrorMessageEqualWhitespaceAndIsVisibleDataGridEqualFalse()
        {
            Assert.Equal(" ", _viewModel.ErrorMessage);
            Assert.False(_viewModel.IsVisibleDataGrid);
        }

        [Fact]
        public void ShouldGetResultAsEmptyTableAndEmptyErrorMessage()
        {
            _viewModel.CommandText = CommandTextResultEmpty;
            _viewModel.Run.Execute(null);
            Assert.Equal(Table.Empty, _viewModel.Table);
            Assert.Equal(string.Empty, _viewModel.ErrorMessage);
        }

        [Fact]
        public void ShouldGetResultAsEmptyTableAndNonEmptyErrorMessage()
        {
            _viewModel.CommandText = CommandTextResultThrowException;
            _viewModel.Run.Execute(null);
            Assert.Equal(Table.Empty, _viewModel.Table);
            Assert.Equal(ErrorMessage, _viewModel.ErrorMessage);
        }

        [Theory]
        [MemberData(nameof(TableNamesTestData))]
        public void ShouldTableNameClickPassedSelectAllFromTableQuery(string tableName)
        {
            var commandText = $"SELECT * FROM {tableName};";
            _viewModel.TableNameClick.Execute(tableName);
            Assert.Equal(commandText, _viewModel.CommandText);
        }
    }
}
