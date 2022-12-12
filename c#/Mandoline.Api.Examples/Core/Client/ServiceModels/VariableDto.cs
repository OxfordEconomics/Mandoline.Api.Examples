namespace Core.Client.ServiceModels;

/// <summary>
/// internal representation of a Variable / Indicator eg GDP, CPI.
/// </summary>
public class VariableDto
{
    /// <summary>
    /// Databank code of databank this variable belongs to.
    /// </summary>
    public string DatabankCode { get; set; }

    /// <summary>
    /// Product type this variable belongs to.
    /// </summary>
    public string ProductTypeCode { get; set; }

    /// <summary>
    /// Variable code.
    /// </summary>
    public string VariableCode { get; set; }

    /// <summary>
    /// Human readable name for this variable.
    /// </summary>
    public string VariableName { get; set; }
}
