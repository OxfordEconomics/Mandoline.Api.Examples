namespace Core.Client.ServiceModels;

public enum FileFormat
{
    Excel,
    Csv,
}

public class ControllerDownloadRequestDto
{
    /// <summary>
    /// Gets or sets array of selections.
    /// </summary>
    public SelectionDto[] selections { get; set; }

    /// <summary>
    /// Gets or sets file format.
    /// </summary>
    public FileFormat format { get; set; }

    /// <summary>
    /// Gets or sets download request name.
    /// </summary>
    public string name { get; set; }
}
