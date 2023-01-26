namespace Core.Client.ServiceModels;

/// <summary>
/// Location class.
/// </summary>
public class RegionDto
{
    /// <summary>
    /// Gets or sets code of region, unique within containing databank.
    /// </summary>
    public string RegionCode { get; set; }

    /// <summary>
    /// Gets or sets human readable name for this region.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets databank code of containing databank.
    /// </summary>
    public string DatabankCode { get; set; }
}
