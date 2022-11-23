
namespace Core.Client.ServiceModels
{
    public class SelectionVariableDto
    {        
        public string VariableCode { get; set; }

        public string ProductTypeCode { get; set; }

        public string[] MeasureCodes { get; set; }

        private string ToIdString()
        {
            return ProductTypeCode + "¬" + VariableCode + "¬" + string.Join(",", MeasureCodes);
        }

    }
}
