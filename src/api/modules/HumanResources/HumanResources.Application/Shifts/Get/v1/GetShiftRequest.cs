namespace FSH.Starter.WebApi.HumanResources.Application.Shifts.Get.v1;

public sealed record GetShiftRequest(DefaultIdType Id) : IRequest<ShiftResponse>;

