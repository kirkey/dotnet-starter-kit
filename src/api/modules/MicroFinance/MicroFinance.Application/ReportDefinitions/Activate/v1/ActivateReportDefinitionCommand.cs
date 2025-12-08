using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.ReportDefinitions.Activate.v1;

public sealed record ActivateReportDefinitionCommand(DefaultIdType Id) : IRequest<ActivateReportDefinitionResponse>;
