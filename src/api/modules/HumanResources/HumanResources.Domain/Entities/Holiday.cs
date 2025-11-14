using FSH.Starter.WebApi.HumanResources.Domain.Events;

namespace FSH.Starter.WebApi.HumanResources.Domain.Entities;

/// <summary>
/// Represents company holidays and special days.
/// Can be fixed date or recurring holidays.
/// </summary>
public class Holiday : AuditableEntity, IAggregateRoot
{
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
        if (month < 1 || month > 12)
            throw new ArgumentException("Month must be 1-12.", nameof(month));

        if (day < 1 || day > 31)
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
}

