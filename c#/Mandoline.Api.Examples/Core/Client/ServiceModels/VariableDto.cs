namespace Core.Client.ServiceModels;

/// <summary>
/// internal representation of a Variable / Indicator eg GDP, CPI.
/// </summary>
public class VariableDto
{
    /// <summary>
    /// Gets or sets databank code of databank this variable belongs to.
    /// </summary>
    public string DatabankCode { get; set; }

    /// <summary>
    /// Gets or sets product type this variable belongs to.
    /// </summary>
    public string ProductTypeCode { get; set; }

    /// <summary>
    /// Gets or sets variable code.
    /// </summary>
    public string VariableCode { get; set; }

    /// <summary>
    /// Gets or sets human readable name for this variable.
    /// </summary>
    public string VariableName { get; set; }
}
