namespace FSH.Starter.WebApi.HumanResources.Application.Holidays.Update.v1;

public sealed record UpdateHolidayCommand(
    DefaultIdType Id,
    [property: DefaultValue(null)] string? HolidayName = null,
    [property: DefaultValue(null)] DateTime? HolidayDate = null,
    [property: DefaultValue(null)] bool? IsPaid = null,
    [property: DefaultValue(null)] bool? IsRecurringAnnually = null,
    [property: DefaultValue(null)] string? Description = null) : IRequest<UpdateHolidayResponse>;

