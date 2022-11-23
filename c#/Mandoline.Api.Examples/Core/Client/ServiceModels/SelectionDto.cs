using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Core.Client.ServiceModels
{
    public enum FrequencyEnum { Annual = 0, Quarterly = 1, Both = 2 }
    public enum SequenceEnum { EarliestToLatest = 0, LatestToEarliest = 1 }
    public enum OrderEnum { LocationIndicator = 0, IndicatorLocation = 1 }
    public enum SortOrderEnum { AlphabeticalOrder = 0, TreeOrder = 1 }

    public enum SelectionTypeEnum { QuerySelection = 0, DataseriesSelection = 1}
    public enum ListingTypeEnum { Hidden = 0, Private = 1, Company = 2, Public = 3, Shared = 4 }


    // TODO: trim down

    // [Serializable, XmlRoot("Selection", Namespace = "http://www.oxfordeconomics.com/schemas/Core.Client.Models")]
    [Serializable, XmlRoot("Selection", Namespace = "http://www.oxfordeconomics.com/schemas/Mandoline.Domain.Models")]    
    public class SelectionDto : GeneralSelectionDto
    {        
        /// <summary>
        /// Unique and persistant id for this saved selection
        /// </summary>
        public Guid Id { get; set; }        

        public string DatabankCode { get; set; }

        public string MeasureCode { get; set; }

        public OrderEnum Order { get; set; }

        public SequenceEnum Sequence { get; set; }
        
        public bool StackedQuarters { get; set; }

        [XmlIgnore]
        public IEnumerable<SelectionVariableDto> Variables { get; set; }

        [XmlIgnore]
        public IEnumerable<SelectionRegionDto> Regions { get; set; }        
        
        public bool GroupingMode { get; set; }

        public bool IsTemporarySelection { get; set; }
        
        public DateTime LastUpdate { get; set; }

        /// <summary>
        /// Link to download this selection
        /// </summary>
        public string DownloadUrl { get; set; }

        public override SelectionTypeEnum SelectionType
        {
            get { return SelectionTypeEnum.QuerySelection; }
        }

        public SelectionDto()
        {
            Regions = new SelectionRegionDto[] { };
            Variables = new SelectionVariableDto[] { };
        }

    }
}
