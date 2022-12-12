using System;

namespace Core.Client.ServiceModels;

public class ControllerDownloadResponseDto
{
    /// <summary>
    /// Url to poll to find when download is ready.
    /// </summary>
    public string ReadyUrl { get; set; }
}
