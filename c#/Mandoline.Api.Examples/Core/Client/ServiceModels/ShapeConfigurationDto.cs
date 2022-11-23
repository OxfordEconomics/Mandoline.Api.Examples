
namespace Core.Client.ServiceModels
{
    public enum FormatEnum { Default = 0, Narrow = 2, Datafeed = 3 }

    public class ShapeConfigurationDto
    {
        public bool Pivot { get; set; }
        public bool StackedQuarters { get; set; }
        public FrequencyEnum Frequency { get; set; }

        public FormatEnum Format { get; set; }

    }
}
