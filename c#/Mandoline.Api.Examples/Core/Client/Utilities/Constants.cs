using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Core.Client;

/// <summary>
/// General constants without a better home.
/// </summary>
public class Constants
{
    /// <summary>
    /// Request Property containing current user.
    /// </summary>
    public const string REQUEST_USER_PROPERTY = "User";

    /// <summary>
    /// ApiKey field sent in Headers or query string.
    /// </summary>
    public const string API_KEY_KEY = "Api-Key";

    /// <summary>
    /// Identifier for the client using the api.
    /// </summary>
    public const string CLIENT_KEY = "Client-Version";
}