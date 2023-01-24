using Core.Client.ServiceModels;

namespace Core.Client.Models;

public class DashboardWidget : DashboardWidgetDto, IApiModel
{
    /// <inheritdoc/>
    public ApiClient Client { get; set; }
}
