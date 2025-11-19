namespace FSH.Starter.WebApi.HumanResources.Application.Holidays.Delete.v1;

/// <summary>
/// Response for deleting a holiday.
/// </summary>
/// <param name="Id">The identifier of the deleted holiday.</param>
public sealed record DeleteHolidayResponse(DefaultIdType Id);

