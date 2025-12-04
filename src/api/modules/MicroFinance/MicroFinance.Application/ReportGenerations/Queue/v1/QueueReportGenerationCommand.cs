using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.ReportGenerations.Queue.v1;

public sealed record QueueReportGenerationCommand(
    Guid ReportDefinitionId,
    string Trigger,
    string OutputFormat,
    Guid? RequestedByUserId = null,
    string? Parameters = null,
    DateOnly? StartDate = null,
    DateOnly? EndDate = null,
    Guid? BranchId = null) : IRequest<QueueReportGenerationResponse>;
