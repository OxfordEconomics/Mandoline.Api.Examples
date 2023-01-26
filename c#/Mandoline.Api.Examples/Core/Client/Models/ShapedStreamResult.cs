using System.Collections.Generic;
using Core.Client.ServiceModels;

namespace Core.Client.Models;

public class ShapedStreamResult
{
    public IEnumerable<IEnumerable<ShapeCellDto>> Rows { get; set; }

    public long RowCount { get; set; }

    public long ColumnCount { get; set; }
}