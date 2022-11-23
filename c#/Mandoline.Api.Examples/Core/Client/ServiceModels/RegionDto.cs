
namespace Core.Client.ServiceModels
{
    /// <summary>
    /// Location class
    /// </summary>
    public class RegionDto
    {
        /// <summary>
        /// code of region, unique within containing databank
        /// </summary>
        public string RegionCode { get; set; }

        /// <summary>
        /// Human readable name for this region
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Databank code of containing databank
        /// </summary>
        public string DatabankCode { get; set; }

    }
}
