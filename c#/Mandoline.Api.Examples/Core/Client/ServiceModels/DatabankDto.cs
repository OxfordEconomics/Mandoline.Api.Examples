using System.Collections.Generic;

namespace Core.Client.ServiceModels
{    
    /// <summary>
    /// Databank
    /// </summary>
    public class DatabankDto
    {
        /// <summary>
        /// The name of the databank
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The databank code for this databank
        /// </summary>
        public string DatabankCode { get; set; }        

        /// <summary>
        /// The start year for this databank
        /// </summary>
        public int? StartYear { get; set; }

        /// <summary>
        /// The end year for this databank
        /// </summary>
        public int? EndYear { get; set; }

        /// <summary>
        /// Does this databank contain quarterly data
        /// </summary>
        public bool? HasQuarterlyData { get; set; }

        /// <summary>
        /// All available trees for this databank
        /// </summary>
        public IEnumerable<TreeLinkDto> Trees { get; set; }

        /// <summary>
        /// Url to this databank entity
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Url to the map entities for this databank
        /// </summary>
        public string MapUrl { get; set; }

        /// <summary>
        /// Does the user have access to this databank
        /// </summary>
        public bool HasAccess { get; set; }

        /// <summary>
        /// Databank columns available on this databank
        /// </summary>
        public IEnumerable<DatabankColumnDto> DatabankColumns { get; set; }
    }
}