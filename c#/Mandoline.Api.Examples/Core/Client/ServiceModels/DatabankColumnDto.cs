namespace Core.Client;

/// <summary>
/// Databank column information.
/// </summary>
public class DatabankColumnDto
{
    /// <summary>
    /// Gets or sets human readable column name.
    /// </summary>
    public string ColumnName { get; set; }

    /// <summary>
    /// Gets or sets display Order.
    /// </summary>
    public int DisplayOrder { get; set; }

    /// <summary>
    /// Gets or sets metadata field to find values.
    /// </summary>
    public string MetadataFieldName { get; set; }
}
