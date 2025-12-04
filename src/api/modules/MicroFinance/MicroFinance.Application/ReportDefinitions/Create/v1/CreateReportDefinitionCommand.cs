using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.ReportDefinitions.Create.v1;

public sealed record CreateReportDefinitionCommand(
    string Code,
    string Name,
    string Category,
    string OutputFormat,
    string? Description = null,
    string? ParametersDefinition = null,
    string? Query = null,
    string? LayoutTemplate = null) : IRequest<CreateReportDefinitionResponse>;
