using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Core.Client.Models;

public class User : ServiceModels.UserDto, IApiModel
{
    /// <inheritdoc/>
    public ApiClient Client { get; set; }

    /// <summary>
    /// Gets links to users saved selections.
    /// </summary>
    [JsonIgnore]
    public new IEnumerable<ResourceLink<Selection>> SavedSelections
    {
        get
        {
            if (base.SavedSelections == null)
            {
                return new ResourceLink<Selection>[] { };
            }

            return from s in base.SavedSelections
                   select new ResourceLink<Selection>(this.Client)
                   {
                       Id = s.Id,
                       Name = s.Name,
                       Url = s.Url,
                   };
        }
    }
}
