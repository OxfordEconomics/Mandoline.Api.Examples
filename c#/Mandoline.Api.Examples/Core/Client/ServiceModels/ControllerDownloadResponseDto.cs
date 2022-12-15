using System;

namespace Core.Client.ServiceModels;

public class ControllerDownloadResponseDto
{
    /// <summary>
    /// Gets or sets url to poll to find when download is ready.
    /// </summary>
    public string ReadyUrl { get; set; }
}
