using System;

namespace Core.Client.ServiceModels;

public class DashboardWidgetDto
{
    /// <summary>
    /// Gets or sets unique identifier (Guid) for this dashboard widget.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets id of selection associated with this widget.
    /// </summary>
    public Guid? SelectionId { get; set; }
}
