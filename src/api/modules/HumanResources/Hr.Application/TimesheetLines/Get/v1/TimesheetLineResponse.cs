namespace FSH.Starter.WebApi.HumanResources.Application.TimesheetLines.Get.v1;

/// <summary>
/// Response containing timesheet line details.
/// </summary>
public sealed record TimesheetLineResponse
{
    /// <summary>Timesheet line ID.</summary>
    public DefaultIdType Id { get; init; }

    /// <summary>Parent timesheet ID.</summary>
    public DefaultIdType TimesheetId { get; init; }

    /// <summary>Work date.</summary>
    public DateTime WorkDate { get; init; }

    /// <summary>Regular hours worked.</summary>
    public decimal RegularHours { get; init; }

    /// <summary>Overtime hours worked.</summary>
    public decimal OvertimeHours { get; init; }

    /// <summary>Total hours (computed).</summary>
    public decimal TotalHours { get; init; }

    /// <summary>Project ID for allocation.</summary>
    public string? ProjectId { get; init; }

    /// <summary>Task description.</summary>
    public string? TaskDescription { get; init; }

    /// <summary>Whether billable to customer.</summary>
    public bool IsBillable { get; init; }

    /// <summary>Billing rate per hour.</summary>
    public decimal? BillingRate { get; init; }
}

