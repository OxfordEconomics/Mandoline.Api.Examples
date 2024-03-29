﻿using System;

namespace Core.Client.ServiceModels;

/// <summary>
/// Dashboard model.
/// </summary>
public class DashboardDto
{
    /// <summary>
    /// Gets or sets unique identifier (Guid) for this dashboard.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets human readable name for this dashboard.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets display order.
    /// </summary>
    public int DisplayOrder { get; set; }

    /// <summary>
    /// Gets or sets contact Id of dashboard owner.
    /// </summary>
    public string OwnerContactId { get; set; }

    public bool IsValid()
    {
        if (string.IsNullOrWhiteSpace(this.Name))
        {
            return false;
        }

        return true;
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return (this.Name + "¬" + this.DisplayOrder).GetHashCode();
    }
}
