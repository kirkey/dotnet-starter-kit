namespace FSH.Starter.WebApi.HumanResources.Application.Holidays.Get.v1;

/// <summary>
/// Response object for Holiday entity with Philippines Labor Code compliance fields.
/// </summary>
public sealed record HolidayResponse
{
    /// <summary>
    /// Gets the unique identifier of the holiday.
    /// </summary>
    public DefaultIdType Id { get; init; }

    /// <summary>
    /// Gets the name of the holiday.
    /// </summary>
    public string HolidayName { get; init; } = default!;

    /// <summary>
    /// Gets the date of the holiday.
    /// </summary>
    public DateTime HolidayDate { get; init; }

    /// <summary>
    /// Gets a value indicating whether the holiday is paid.
    /// </summary>
    public bool IsPaid { get; init; }

    /// <summary>
    /// Gets a value indicating whether the holiday recurs annually.
    /// </summary>
    public bool IsRecurringAnnually { get; init; }

    /// <summary>
    /// Gets the day of month for recurring holidays (1-31).
    /// </summary>
    public int? RecurringMonthDay { get; init; }

    /// <summary>
    /// Gets the month for recurring holidays (1-12).
    /// </summary>
    public int? RecurringMonth { get; init; }

    /// <summary>
    /// Gets the description of the holiday.
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    /// Gets a value indicating whether the holiday is active.
    /// </summary>
    public bool IsActive { get; init; }

    // Philippines-Specific Fields

    /// <summary>
    /// Gets the holiday type per Philippine regulations.
    /// RegularPublicHoliday: 100% pay premium if worked.
    /// SpecialNonWorkingDay: 30% pay premium if worked.
    /// </summary>
    public string Type { get; init; } = "RegularPublicHoliday";

    /// <summary>
    /// Gets the pay rate multiplier if employee works on this holiday.
    /// RegularPublicHoliday: 1.0 (100% additional = 200% total)
    /// SpecialNonWorkingDay: 0.3 (30% additional = 130% total)
    /// </summary>
    public decimal PayRateMultiplier { get; init; } = 1.0m;

    /// <summary>
    /// Gets a value indicating whether this holiday's date changes yearly (moveable).
    /// Examples: Easter, Holy Week, National Heroes Day.
    /// </summary>
    public bool IsMoveable { get; init; }

    /// <summary>
    /// Gets the rule for calculating moveable holiday date.
    /// Examples: "Easter-based", "Last Monday of August".
    /// </summary>
    public string? MoveableRule { get; init; }

    /// <summary>
    /// Gets a value indicating whether holiday applies nationwide.
    /// True: Nationwide (all provinces/cities)
    /// False: Regional (specific provinces/LGUs only)
    /// </summary>
    public bool IsNationwide { get; init; } = true;

    /// <summary>
    /// Gets comma-separated list of regions/provinces where holiday applies (if not nationwide).
    /// Examples: "BARMM", "NCR,Region IV-A", "Davao City"
    /// </summary>
    public string? ApplicableRegions { get; init; }
}

