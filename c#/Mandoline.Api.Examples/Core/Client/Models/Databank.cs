using Core.Client.ServiceModels;
using System.Collections.Generic;

namespace Core.Client.Models
{
    public class Databank : DatabankDto
    {                
        public ApiClient apiClient;

        public Databank(ApiClient apiClient)
        {            
            this.apiClient = apiClient;
        }

        public new IEnumerable<TreeLink> Trees { get; set; }
       
    }
}
