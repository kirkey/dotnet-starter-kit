namespace FSH.Starter.WebApi.HumanResources.Application.Shifts.Delete.v1;

public sealed record DeleteShiftCommand(DefaultIdType Id) : IRequest<DeleteShiftResponse>;

