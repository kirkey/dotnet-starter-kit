namespace FSH.Starter.WebApi.HumanResources.Application.Holidays.Create.v1;

public sealed record CreateHolidayCommand(
    [property: DefaultValue("New Year")] string HolidayName,
    [property: DefaultValue("2025-01-01")] DateTime HolidayDate,
    [property: DefaultValue(true)] bool IsPaid = true,
    [property: DefaultValue(false)] bool IsRecurringAnnually = false,
    [property: DefaultValue(null)] string? Description = null) : IRequest<CreateHolidayResponse>;

