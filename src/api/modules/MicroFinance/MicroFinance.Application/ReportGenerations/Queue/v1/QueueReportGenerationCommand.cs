using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.ReportGenerations.Queue.v1;

public sealed record QueueReportGenerationCommand(
    DefaultIdType ReportDefinitionId,
    string Trigger,
    string OutputFormat,
    DefaultIdType? RequestedByUserId = null,
    string? Parameters = null,
    DateOnly? StartDate = null,
    DateOnly? EndDate = null,
    DefaultIdType? BranchId = null) : IRequest<QueueReportGenerationResponse>;
