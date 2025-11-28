namespace FSH.Starter.Blazor.Client.Pages.Hr.Holidays;

/// <summary>
/// View model for Holiday CRUD operations.
/// Contains all properties needed for create and update operations.
/// </summary>
public class HolidayViewModel
{
    /// <summary>
    /// Gets or sets the holiday ID.
    /// </summary>
    public DefaultIdType Id { get; set; }

    /// <summary>
    /// Gets or sets the holiday name.
    /// </summary>
    public string? HolidayName { get; set; }

    /// <summary>
    /// Gets or sets the holiday date.
    /// </summary>
    public DateTime? HolidayDate { get; set; } = DateTime.Today;

    /// <summary>
    /// Gets or sets whether the holiday is paid.
    /// </summary>
    public bool IsPaid { get; set; } = true;

    /// <summary>
    /// Gets or sets whether the holiday recurs annually.
    /// </summary>
    public bool IsRecurringAnnually { get; set; }

    /// <summary>
    /// Gets or sets the holiday description.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the holiday type (RegularPublicHoliday or SpecialNonWorkingDay).
    /// </summary>
    public string? Type { get; set; } = "RegularPublicHoliday";

    /// <summary>
    /// Gets or sets the pay rate multiplier for working on this holiday.
    /// </summary>
    public decimal PayRateMultiplier { get; set; } = 1.0m;

    /// <summary>
    /// Gets or sets whether the holiday date is moveable (e.g., Easter).
    /// </summary>
    public bool IsMoveable { get; set; }

    /// <summary>
    /// Gets or sets the rule for calculating moveable holiday dates.
    /// </summary>
    public string? MoveableRule { get; set; }

    /// <summary>
    /// Gets or sets whether the holiday applies nationwide.
    /// </summary>
    public bool IsNationwide { get; set; } = true;

    /// <summary>
    /// Gets or sets the applicable regions (if not nationwide).
    /// </summary>
    public string? ApplicableRegions { get; set; }

    /// <summary>
    /// Gets or sets whether the holiday is active.
    /// </summary>
    public bool IsActive { get; set; } = true;
}
