namespace Core.Client.ServiceModels;

public class GeneralSelectionDto
{
    /// <summary>
    /// Gets or sets human readable name for this selection.
    /// </summary>
    public string Name { get; set; }

    public int StartYear { get; set; }

    public int EndYear { get; set; }

    public int Precision { get; set; }

    public FrequencyEnum Frequency { get; set; }

    public virtual SelectionTypeEnum SelectionType { get; set; }

    /// <summary>
    /// Gets or sets is the selection listed among saved selections.
    /// </summary>
    public ListingTypeEnum ListingType { get; set; }
}
