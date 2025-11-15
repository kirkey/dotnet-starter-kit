namespace FSH.Starter.WebApi.HumanResources.Application.TimesheetLines.Create.v1;

/// <summary>
/// Create a new timesheet line (daily entry).
/// </summary>
public sealed record CreateTimesheetLineCommand : IRequest<CreateTimesheetLineResponse>
{
    /// <summary>Parent timesheet ID.</summary>
    public DefaultIdType TimesheetId { get; init; }

    /// <summary>Work date.</summary>
    public DateTime WorkDate { get; init; }

    /// <summary>Regular hours worked.</summary>
    public decimal RegularHours { get; init; }

    /// <summary>Overtime hours worked.</summary>
    public decimal OvertimeHours { get; init; }

    /// <summary>Project ID.</summary>
    public string? ProjectId { get; init; }

    /// <summary>Task description.</summary>
    public string? TaskDescription { get; init; }

    /// <summary>Billing rate.</summary>
    public decimal? BillingRate { get; init; }
}
