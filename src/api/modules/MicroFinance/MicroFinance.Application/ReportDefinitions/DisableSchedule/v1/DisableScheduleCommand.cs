using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.ReportDefinitions.DisableSchedule.v1;

public sealed record DisableScheduleCommand(DefaultIdType Id) : IRequest<DisableScheduleResponse>;

