using SerializeDictionary;

namespace Core.Client.ServiceModels;

/// <summary>
/// web layer data series.
/// </summary>
public class DataseriesDto
{
    /// <summary>
    /// Gets or sets databank code of databank the series belongs to.
    /// </summary>
    public string DatabankCode { get; set; }

    /// <summary>
    /// Gets or sets region code of dataseries.
    /// </summary>
    public string LocationCode { get; set; }

    /// <summary>
    /// Gets or sets variable code of data series.
    /// </summary>
    public string VariableCode { get; set; }

    public SerializableDictionary2<string, double?> AnnualData { get; set; }

    public SerializableDictionary2<string, double?> QuarterlyData { get; set; }
}