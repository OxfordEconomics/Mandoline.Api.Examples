using System.Collections.Generic;
using Core.Client.ServiceModels;

namespace Core.Client.Models;

public class Databank : DatabankDto
{
    private ApiClient ApiClient;

    public Databank(ApiClient apiClient)
    {
        this.ApiClient = apiClient;
    }

    public new IEnumerable<TreeLink> Trees { get; set; }
}
