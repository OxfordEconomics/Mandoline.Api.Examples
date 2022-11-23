using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;


namespace Core.Client.Models
{
    public class User : ServiceModels.UserDto, ApiModel
    {
        public ApiClient Client { get; set; }

        /// <summary>
        /// Links to users saved selections
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
                       select new ResourceLink<Selection>(Client)
                       {
                           Id = s.Id,
                           Name = s.Name,
                           Url = s.Url
                       };
            }
        }
       
    }
}
