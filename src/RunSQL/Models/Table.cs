using System.Collections.Generic;

namespace RunSQL.Models
{
    public class Table
    {
        public static readonly Table Empty = new();

        public IReadOnlyList<string> Headers { get; init; }

        public IReadOnlyList<TableRow> Rows { get; init; }

        public Table()
        {
            Headers = new List<string>();
            Rows = new List<TableRow>();
        }
    }
}
