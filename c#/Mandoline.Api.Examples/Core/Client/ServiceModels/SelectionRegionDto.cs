namespace Core.Client.ServiceModels;

public class SelectionRegionDto
{
    /// <summary>
    /// Gets or sets databank containing region.
    /// </summary>
    public string DatabankCode { get; set; }

    /// <summary>
    /// Gets or sets region code within databank.
    /// </summary>
    public string RegionCode { get; set; }
}
