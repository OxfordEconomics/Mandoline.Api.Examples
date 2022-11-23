
namespace Core.Client.ServiceModels
{
    public enum FileFormat { Excel, Csv };

    public class ControllerDownloadRequestDto
    {
        /// <summary>
        /// array of selections
        /// </summary>
        public SelectionDto[] selections { get; set; }

        /// <summary>
        /// File format
        /// </summary>
        public FileFormat format { get; set; }

        /// <summary>
        /// download request name
        /// </summary>
        public string name { get; set; }
    }
}
