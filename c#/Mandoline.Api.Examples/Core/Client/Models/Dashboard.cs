using Core.Client.ServiceModels;

namespace Core.Client.Models;

public class Dashboard : DashboardDto, IApiModel
{
    /// <inheritdoc/>
    public ApiClient Client { get; set; }
}
