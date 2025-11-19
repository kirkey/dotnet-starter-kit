namespace FSH.Starter.WebApi.HumanResources.Application.Holidays.Get.v1;

/// <summary>
/// Request to get a holiday by its identifier.
/// </summary>
public sealed record GetHolidayRequest(DefaultIdType Id) : IRequest<HolidayResponse>;


