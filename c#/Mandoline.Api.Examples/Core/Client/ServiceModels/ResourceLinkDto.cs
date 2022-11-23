using System;

namespace Core.Client.ServiceModels
{
    /// <summary>
    /// Link to a resource
    /// </summary>
    public class ResourceLinkDto
    {
        /// <summary>
        /// Selection id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Selection name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Selection url
        /// </summary>
        public string Url { get; set; }
    }
}