using Core.Client.ServiceModels;

namespace Core.Client.Models
{
    public class Dashboard : DashboardDto, ApiModel
    {
        public ApiClient Client { get; set; }
    }
}
