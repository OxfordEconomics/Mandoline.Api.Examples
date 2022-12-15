using System.Collections.Generic;

namespace Core.Client.ServiceModels;

/// <summary>
/// Databank.
/// </summary>
public class DatabankDto
{
    /// <summary>
    /// Gets or sets the name of the databank.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the databank code for this databank.
    /// </summary>
    public string DatabankCode { get; set; }

    /// <summary>
    /// Gets or sets the start year for this databank.
    /// </summary>
    public int? StartYear { get; set; }

    /// <summary>
    /// Gets or sets the end year for this databank.
    /// </summary>
    public int? EndYear { get; set; }

    /// <summary>
    /// Gets or sets does this databank contain quarterly data.
    /// </summary>
    public bool? HasQuarterlyData { get; set; }

    /// <summary>
    /// Gets or sets all available trees for this databank.
    /// </summary>
    public IEnumerable<TreeLinkDto> Trees { get; set; }

    /// <summary>
    /// Gets or sets url to this databank entity.
    /// </summary>
    public string Url { get; set; }

    /// <summary>
    /// Gets or sets url to the map entities for this databank.
    /// </summary>
    public string MapUrl { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether does the user have access to this databank.
    /// </summary>
    public bool HasAccess { get; set; }

    /// <summary>
    /// Gets or sets databank columns available on this databank.
    /// </summary>
    public IEnumerable<DatabankColumnDto> DatabankColumns { get; set; }
}