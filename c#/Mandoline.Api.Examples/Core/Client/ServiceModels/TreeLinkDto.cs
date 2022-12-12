namespace Core.Client.ServiceModels;

/// <summary>
/// Presentational tree for a data facet.
/// </summary>
public class TreeLinkDto
{
    /// <summary>
    /// Name of the tree.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Tree Code.
    /// </summary>
    public string TreeCode { get; set; }

    /// <summary>
    /// Url to this tree.
    /// </summary>
    public string Url { get; set; }
}