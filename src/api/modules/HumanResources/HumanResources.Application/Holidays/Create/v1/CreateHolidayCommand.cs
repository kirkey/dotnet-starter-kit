namespace FSH.Starter.WebApi.HumanResources.Application.Holidays.Create.v1;

/// <summary>
/// Command to create a new holiday with Philippines Labor Code compliance.
/// Includes holiday type, pay rate multiplier, and regional applicability.
/// </summary>
public sealed record CreateHolidayCommand(
    [property: DefaultValue("New Year's Day")] string HolidayName,
    [property: DefaultValue("2025-01-01")] DateTime HolidayDate,
    
    // Basic Holiday Configuration
    [property: DefaultValue(true)] bool IsPaid = true,
    [property: DefaultValue(false)] bool IsRecurringAnnually = false,
    [property: DefaultValue(null)] string? Description = null,
    
    // Philippines-Specific: Holiday Classification per Labor Code
    [property: DefaultValue("RegularPublicHoliday")] string Type = "RegularPublicHoliday",
    [property: DefaultValue(1.0)] decimal PayRateMultiplier = 1.0m,
    
    // Philippines-Specific: Moveable Holidays (Easter, Holy Week, etc.)
    [property: DefaultValue(false)] bool IsMoveable = false,
    [property: DefaultValue(null)] string? MoveableRule = null,
    
    // Philippines-Specific: Regional Applicability
    [property: DefaultValue(true)] bool IsNationwide = true,
    [property: DefaultValue(null)] string? ApplicableRegions = null
) : IRequest<CreateHolidayResponse>;

