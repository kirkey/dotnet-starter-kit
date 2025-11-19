namespace FSH.Starter.WebApi.HumanResources.Application.Holidays.Delete.v1;

/// <summary>
/// Command to delete a holiday by its identifier.
/// </summary>
public sealed record DeleteHolidayCommand(DefaultIdType Id) : IRequest<DeleteHolidayResponse>;


