using System.Collections.Generic;

namespace Core.Client.ServiceModels;

/// <summary>
/// Mandoline user.
/// </summary>
// TODO: extend domain user
public class UserDto
{
    /// <summary>
    /// Gets or sets user surname.
    /// </summary>
    public string LastName { get; set; }

    /// <summary>
    /// Gets or sets user forename.
    /// </summary>
    public string FirstName { get; set; }

    /// <summary>
    /// Gets or sets links to users saved selections.
    /// </summary>
    public IEnumerable<ResourceLinkDto> SavedSelections { get; set; }

    /// <summary>
    /// Gets or sets users api key.
    /// </summary>
    public string ApiKey { get; set; }
}