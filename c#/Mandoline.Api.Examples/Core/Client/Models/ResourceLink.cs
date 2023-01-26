using Core.Client.ServiceModels;

namespace Core.Client.Models;

public class ResourceLink<T> : ResourceLinkDto
    where T : class, IApiModel
{
    private readonly ApiClient client;

    public ResourceLink(ApiClient client)
    {
        this.client = client;
    }
}
