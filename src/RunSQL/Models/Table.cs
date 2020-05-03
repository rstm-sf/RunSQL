using System.Collections.Generic;

namespace RunSQL.Models
{
    internal class Table
    {
        public static readonly Table Empty = new Table
        {
            Headers = new List<string>(),
            Rows = new List<TableRow>(),
        };

        public IReadOnlyList<string> Headers { get; set; }

        public IReadOnlyList<TableRow> Rows { get; set; }
    }
}
