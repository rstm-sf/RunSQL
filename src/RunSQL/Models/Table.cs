using System.Collections.Generic;

namespace RunSQL.Models
{
    internal class Table
    {
        public IReadOnlyList<string> Headers { get; set; }

        public IReadOnlyList<TableRow> Rows { get; set; }
    }
}
