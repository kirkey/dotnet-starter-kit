using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.ReportGenerations.Cancel.v1;

/// <summary>
/// Command to cancel a report generation.
/// </summary>
public sealed record CancelReportGenerationCommand(DefaultIdType Id, string? Reason = null) 
    : IRequest<CancelReportGenerationResponse>;

