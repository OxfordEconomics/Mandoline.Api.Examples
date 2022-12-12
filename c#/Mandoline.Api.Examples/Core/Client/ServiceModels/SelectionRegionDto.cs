namespace Core.Client.ServiceModels;

public class SelectionRegionDto
{
    /// <summary>
    /// Databank containing region.
    /// </summary>
    public string DatabankCode { get; set; }

    /// <summary>
    /// Region code within databank.
    /// </summary>
    public string RegionCode { get; set; }
}
