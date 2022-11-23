using Core.Client.ServiceModels;

namespace Core.Client.Models
{
    public class Selection : SelectionDto, ApiModel
    {        
        public ApiClient Client { get; set; }
    }
}
