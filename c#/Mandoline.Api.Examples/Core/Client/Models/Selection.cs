using Core.Client.ServiceModels;

namespace Core.Client.Models;

public class Selection : SelectionDto, IApiModel
{
    /// <inheritdoc/>
    public ApiClient Client { get; set; }
}
