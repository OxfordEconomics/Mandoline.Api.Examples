using System;

namespace Core.Client.ServiceModels;

/// <summary>
/// Link to a resource.
/// </summary>
public class ResourceLinkDto
{
    /// <summary>
    /// Gets or sets selection id.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets selection name.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets selection url.
    /// </summary>
    public string Url { get; set; }
}