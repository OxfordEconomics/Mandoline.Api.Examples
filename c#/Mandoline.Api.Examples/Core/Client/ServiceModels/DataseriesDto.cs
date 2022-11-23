using SerializeDictionary;

namespace Core.Client.ServiceModels
{
    /// <summary>
    /// web layer data series
    /// </summary>    
    public class DataseriesDto
    {
        /// <summary>
        /// databank code of databank the series belongs to
        /// </summary>
        public string DatabankCode { get; set; }

        /// <summary>
        /// Region code of dataseries
        /// </summary>
        public string LocationCode { get; set; }

        /// <summary>
        /// Variable code of data series
        /// </summary>
        public string VariableCode { get; set; }

        public SerializableDictionary2<string, double?> AnnualData { get; set; }
        public SerializableDictionary2<string, double?> QuarterlyData { get; set; }
        
    }

}