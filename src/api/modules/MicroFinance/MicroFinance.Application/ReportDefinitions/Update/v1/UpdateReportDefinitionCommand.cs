using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.ReportDefinitions.Update.v1;

public sealed record UpdateReportDefinitionCommand(
    DefaultIdType Id,
    string? Name,
    string? Description,
    string? ParametersDefinition,
    string? Query,
    string? LayoutTemplate,
    string? OutputFormat,
    string? Notes) : IRequest<UpdateReportDefinitionResponse>;

