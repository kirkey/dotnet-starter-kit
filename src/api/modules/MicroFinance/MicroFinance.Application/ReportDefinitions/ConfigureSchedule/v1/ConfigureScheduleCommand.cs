using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.ReportDefinitions.ConfigureSchedule.v1;

public sealed record ConfigureScheduleCommand(
    DefaultIdType Id,
    string Frequency,
    int? Day,
    TimeOnly? Time,
    string? Recipients) : IRequest<ConfigureScheduleResponse>;

