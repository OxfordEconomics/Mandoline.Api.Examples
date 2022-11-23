using System.Collections.Generic;

namespace Core.Client.ServiceModels
{

    /// <summary>
    /// Mandoline user
    /// </summary>
    // TODO: extend domain user
    public class UserDto
    {

        /// <summary>
        /// user surname
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// user forename
        /// </summary>
        public string FirstName { get; set; }


        /// <summary>
        /// Links to users saved selections
        /// </summary>
        public IEnumerable<ResourceLinkDto> SavedSelections { get; set; }        
        

        /// <summary>
        /// Users api key        
        /// </summary>        
        public string ApiKey { get; set; }        
    }
}