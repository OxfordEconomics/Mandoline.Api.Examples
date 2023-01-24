namespace Core.Client.ServiceModels;

/// <summary>
/// Presentational tree for a data facet.
/// </summary>
public class TreeLinkDto
{
    /// <summary>
    /// Gets or sets name of the tree.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets tree Code.
    /// </summary>
    public string TreeCode { get; set; }

    /// <summary>
    /// Gets or sets url to this tree.
    /// </summary>
    public string Url { get; set; }
}