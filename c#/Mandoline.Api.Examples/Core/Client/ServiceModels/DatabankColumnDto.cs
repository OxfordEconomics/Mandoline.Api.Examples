namespace Core.Client;

/// <summary>
/// Databank column information.
/// </summary>
public class DatabankColumnDto
{
    /// <summary>
    /// Human readable column name.
    /// </summary>
    public string ColumnName { get; set; }

    /// <summary>
    /// Display Order.
    /// </summary>
    public int DisplayOrder { get; set; }

    /// <summary>
    /// Metadata field to find values.
    /// </summary>
    public string MetadataFieldName { get; set; }
}
