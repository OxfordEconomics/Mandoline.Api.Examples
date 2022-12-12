using Core.Client.ServiceModels;

namespace Core.Client.Models;

public class TreeLink : TreeLinkDto
{
    private readonly ApiClient client;

    public TreeLink(ApiClient client)
    {
        this.client = client;
    }
}
