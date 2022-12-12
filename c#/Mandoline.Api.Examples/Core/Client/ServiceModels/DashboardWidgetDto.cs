using System;

namespace Core.Client.ServiceModels;

public class DashboardWidgetDto
{
    /// <summary>
    /// Unique identifier (Guid) for this dashboard widget.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Id of selection associated with this widget.
    /// </summary>
    public Guid? SelectionId { get; set; }
}
