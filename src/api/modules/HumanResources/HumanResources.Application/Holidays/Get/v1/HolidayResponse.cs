namespace FSH.Starter.WebApi.HumanResources.Application.Holidays.Get.v1;

/// <summary>
/// Response object for Holiday entity details.
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
    /// Gets the description of the holiday.
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    /// Gets a value indicating whether the holiday is active.
    /// </summary>
    public bool IsActive { get; init; }
}

