namespace FSH.Starter.WebApi.HumanResources.Application.Holidays.Update.v1;

/// <summary>
/// Command to update holiday with Philippines Labor Code compliance.
/// All fields are optional - only provided fields will be updated.
/// </summary>
public sealed record UpdateHolidayCommand(
    DefaultIdType Id,
    
    // Basic Configuration
    [property: DefaultValue(null)] string? HolidayName = null,
    [property: DefaultValue(null)] DateTime? HolidayDate = null,
    [property: DefaultValue(null)] bool? IsPaid = null,
    [property: DefaultValue(null)] bool? IsRecurringAnnually = null,
    [property: DefaultValue(null)] string? Description = null,
    
    // Philippines-Specific Fields
    [property: DefaultValue(null)] string? Type = null,
    [property: DefaultValue(null)] decimal? PayRateMultiplier = null,
    [property: DefaultValue(null)] bool? IsMoveable = null,
    [property: DefaultValue(null)] string? MoveableRule = null,
    [property: DefaultValue(null)] bool? IsNationwide = null,
    [property: DefaultValue(null)] string? ApplicableRegions = null
) : IRequest<UpdateHolidayResponse>;

