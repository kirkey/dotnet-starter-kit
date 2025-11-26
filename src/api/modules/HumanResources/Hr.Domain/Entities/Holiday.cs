using FSH.Starter.WebApi.HumanResources.Domain.Events;

namespace FSH.Starter.WebApi.HumanResources.Domain.Entities;

/// <summary>
/// Represents company holidays and special days.
/// Can be fixed date or recurring holidays.
/// </summary>
public class Holiday : AuditableEntity, IAggregateRoot
{
    // Domain Constants - Binary Limits (Powers of 2)
    /// <summary>
    /// Maximum length for the holiday name field. (100)
    /// </summary>
    public const int HolidayNameMaxLength = 100;

    /// <summary>
    /// Maximum length for the type field. (50)
    /// </summary>
    public const int TypeMaxLength = 50;

    /// <summary>
    /// Maximum length for the moveable rule field. (100)
    /// </summary>
    public const int MoveableRuleMaxLength = 100;

    /// <summary>
    /// Maximum length for the applicable regions field. (2^8 = 256)
    /// </summary>
    public const int ApplicableRegionsMaxLength = 256;

    /// <summary>
    /// Maximum length for the description field. (2^11 = 2048)
    /// </summary>
    public const int DescriptionMaxLength = 2048;

    private Holiday() { }

    private Holiday(
        DefaultIdType id,
        string holidayName,
        DateTime holidayDate,
        bool isPaid = true,
        bool isRecurringAnnually = false)
    {
        Id = id;
        HolidayName = holidayName;
        HolidayDate = holidayDate;
        IsPaid = isPaid;
        IsRecurringAnnually = isRecurringAnnually;
        IsActive = true;
    }

    /// <summary>
    /// Name of the holiday.
    /// </summary>
    public string HolidayName { get; private set; } = default!;

    /// <summary>
    /// Date of the holiday.
    /// </summary>
    public DateTime HolidayDate { get; private set; }

    /// <summary>
    /// Whether holiday is paid.
    /// </summary>
    public bool IsPaid { get; private set; }

    /// <summary>
    /// Holiday type per Philippine regulations.
    /// RegularPublicHoliday: 100% pay premium if worked (New Year, Labor Day, Christmas, etc).
    /// SpecialNonWorkingDay: 30% pay premium if worked (Black Saturday, local fiestas).
    /// </summary>
    public string Type { get; private set; } = "RegularPublicHoliday";

    /// <summary>
    /// Pay rate multiplier if employee works on this holiday.
    /// RegularPublicHoliday: 1.0 (100% additional pay = 200% total).
    /// SpecialNonWorkingDay: 0.3 (30% additional pay = 130% total).
    /// DoubleHoliday (Regular + Rest Day): 1.3 (130% additional = 230% total).
    /// </summary>
    public decimal PayRateMultiplier { get; private set; } = 1.0m;

    /// <summary>
    /// Whether this is a recurring holiday (annual).
    /// </summary>
    public bool IsRecurringAnnually { get; private set; }

    /// <summary>
    /// Day of month for recurring holidays (1-31).
    /// </summary>
    public int? RecurringMonthDay { get; private set; }

    /// <summary>
    /// Month for recurring holidays (1-12).
    /// </summary>
    public int? RecurringMonth { get; private set; }

    /// <summary>
    /// Whether this holiday's date changes yearly (moveable).
    /// Examples: Easter (lunar calendar), Holy Week, National Heroes Day (last Monday of August).
    /// </summary>
    public bool IsMoveable { get; private set; } = false;

    /// <summary>
    /// Rule for calculating moveable holiday date.
    /// Examples: "Easter-based", "Last Monday of August", "Islamic calendar".
    /// </summary>
    public string? MoveableRule { get; private set; }

    /// <summary>
    /// Whether holiday applies nationwide or is regional.
    /// True: Nationwide (all provinces/cities).
    /// False: Regional (specific provinces/LGUs only).
    /// </summary>
    public bool IsNationwide { get; private set; } = true;

    /// <summary>
    /// Comma-separated list of regions/provinces where holiday applies (if not nationwide).
    /// Examples: "BARMM", "NCR,Region IV-A", "Davao City".
    /// Used for local fiestas, patron saint days, charter days.
    /// </summary>
    public string? ApplicableRegions { get; private set; }

    /// <summary>
    /// Description or notes about the holiday.
    /// </summary>
    public string? Description { get; private set; }

    /// <summary>
    /// Whether this holiday is active.
    /// </summary>
    public bool IsActive { get; private set; }

    /// <summary>
    /// Creates a new holiday.
    /// </summary>
    public static Holiday Create(
        string holidayName,
        DateTime holidayDate,
        bool isPaid = true,
        bool isRecurringAnnually = false)
    {
        if (string.IsNullOrWhiteSpace(holidayName))
            throw new ArgumentException("Holiday name is required.", nameof(holidayName));

        var holiday = new Holiday(
            DefaultIdType.NewGuid(),
            holidayName,
            holidayDate,
            isPaid,
            isRecurringAnnually);

        return holiday;
    }

    /// <summary>
    /// Updates holiday information.
    /// </summary>
    public Holiday Update(
        string? holidayName = null,
        DateTime? holidayDate = null,
        bool? isPaid = null,
        bool? isRecurringAnnually = null)
    {
        if (!string.IsNullOrWhiteSpace(holidayName))
            HolidayName = holidayName;

        if (holidayDate.HasValue)
            HolidayDate = holidayDate.Value;

        if (isPaid.HasValue)
            IsPaid = isPaid.Value;

        if (isRecurringAnnually.HasValue)
            IsRecurringAnnually = isRecurringAnnually.Value;

        return this;
    }

    /// <summary>
    /// Sets the description of the holiday.
    /// </summary>
    public Holiday SetDescription(string? description)
    {
        Description = description;
        return this;
    }

    /// <summary>
    /// Sets recurring pattern for the holiday.
    /// </summary>
    public Holiday SetRecurring(int month, int day)
    {
        if (month is < 1 or > 12)
            throw new ArgumentException("Month must be 1-12.", nameof(month));

        if (day is < 1 or > 31)
            throw new ArgumentException("Day must be 1-31.", nameof(day));

        IsRecurringAnnually = true;
        RecurringMonth = month;
        RecurringMonthDay = day;

        return this;
    }

    /// <summary>
    /// Deactivates the holiday.
    /// </summary>
    public Holiday Deactivate()
    {
        IsActive = false;
        return this;
    }

    /// <summary>
    /// Activates the holiday.
    /// </summary>
    public Holiday Activate()
    {
        IsActive = true;
        return this;
    }

    /// <summary>
    /// Sets holiday type and pay rate per Philippine Labor Code.
    /// </summary>
    public Holiday SetHolidayType(string type, decimal payRateMultiplier)
    {
        if (type != "RegularPublicHoliday" && type != "SpecialNonWorkingDay")
            throw new ArgumentException("Type must be 'RegularPublicHoliday' or 'SpecialNonWorkingDay'.", nameof(type));

        Type = type;
        PayRateMultiplier = payRateMultiplier;
        return this;
    }

    /// <summary>
    /// Sets holiday as moveable (date changes yearly based on calendar rules).
    /// </summary>
    public Holiday SetMoveable(bool isMoveable, string? moveableRule = null)
    {
        IsMoveable = isMoveable;
        MoveableRule = moveableRule;
        return this;
    }

    /// <summary>
    /// Sets regional applicability (nationwide or specific regions/provinces).
    /// </summary>
    public Holiday SetRegionalApplicability(bool isNationwide, string? applicableRegions = null)
    {
        IsNationwide = isNationwide;
        ApplicableRegions = applicableRegions;
        return this;
    }

    /// <summary>
    /// Checks if holiday applies to a specific region/province.
    /// </summary>
    public bool AppliesToRegion(string region)
    {
        if (IsNationwide)
            return true;

        if (string.IsNullOrWhiteSpace(ApplicableRegions))
            return false;

        var regions = ApplicableRegions.Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(r => r.Trim().ToUpperInvariant());

        return regions.Contains(region.Trim().ToUpperInvariant());
    }

    /// <summary>
    /// Calculates holiday pay premium for employee working on this holiday.
    /// </summary>
    public decimal CalculateHolidayPremium(decimal dailyRate, bool isRestDay = false, bool hasOvertime = false)
    {
        // Base premium
        var premium = dailyRate * PayRateMultiplier;

        // Double holiday (regular holiday + rest day)
        if (Type == "RegularPublicHoliday" && isRestDay)
            premium = dailyRate * 1.3m; // 130% additional = 230% total

        // Triple holiday (regular holiday + rest day + overtime)
        if (Type == "RegularPublicHoliday" && isRestDay && hasOvertime)
            premium = dailyRate * 2.0m; // 200% additional = 300% total

        return premium;
    }
}

