using System.Collections.Generic;

namespace RunSQL.Models
{
    public class TableRow
    {
        public IReadOnlyList<object> Fields { get; init; }

        public TableRow()
        {
            Fields = new object[0];
        }
    }
}
