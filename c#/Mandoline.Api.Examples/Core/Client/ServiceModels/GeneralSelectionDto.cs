namespace Core.Client.ServiceModels;

public class GeneralSelectionDto
{
    /// <summary>
    /// Human readable name for this selection.
    /// </summary>
    public string Name { get; set; }

    public int StartYear { get; set; }

    public int EndYear { get; set; }

    public int Precision { get; set; }

    public FrequencyEnum Frequency { get; set; }

    public virtual SelectionTypeEnum SelectionType { get; set; }

    /// <summary>
    /// Is the selection listed among saved selections.
    /// </summary>
    public ListingTypeEnum ListingType { get; set; }
}
