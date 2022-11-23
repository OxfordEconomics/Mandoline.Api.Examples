using Core.Client.ServiceModels;

namespace Core.Client.Models
{
    public class DashboardWidget : DashboardWidgetDto, ApiModel
    {
        public ApiClient Client { get; set; }

    }
}
