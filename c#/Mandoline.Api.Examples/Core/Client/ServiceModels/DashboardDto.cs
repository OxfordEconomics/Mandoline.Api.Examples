using System;

namespace Core.Client.ServiceModels
{  
    /// <summary>
    /// Dashboard model
    /// </summary>
    public class DashboardDto
    {
        /// <summary>
        /// Unique identifier (Guid) for this dashboard
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Human readable name for this dashboard
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Display order
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        /// Contact Id of dashboard owner
        /// </summary>
        public string OwnerContactId { get; set; }


        public bool IsValid()
        {
            if (String.IsNullOrWhiteSpace(Name))
            {
                return false;
            }

            return true;
        }        

        public override int GetHashCode()
        {
            return (this.Name + "¬" + this.DisplayOrder).GetHashCode();
        }
    }
}
