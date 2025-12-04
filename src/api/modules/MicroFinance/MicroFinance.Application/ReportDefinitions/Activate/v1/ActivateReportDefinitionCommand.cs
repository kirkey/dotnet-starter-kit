using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.ReportDefinitions.Activate.v1;

public sealed record ActivateReportDefinitionCommand(Guid Id) : IRequest<ActivateReportDefinitionResponse>;
