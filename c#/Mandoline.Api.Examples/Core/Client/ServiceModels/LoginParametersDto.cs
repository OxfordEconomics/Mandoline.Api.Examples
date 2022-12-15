namespace Core.Client.ServiceModels;

/// <summary>
/// Login parameters.
/// </summary>
public class LoginParametersDto
{
    /// <summary>
    /// Gets or sets username.
    /// </summary>
    public string Username { get; set; }

    /// <summary>
    /// Gets or sets password.
    /// </summary>
    public string Password { get; set; }
}