using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.ReportDefinitions.Deactivate.v1;

public sealed record DeactivateReportDefinitionCommand(DefaultIdType Id) : IRequest<DeactivateReportDefinitionResponse>;

