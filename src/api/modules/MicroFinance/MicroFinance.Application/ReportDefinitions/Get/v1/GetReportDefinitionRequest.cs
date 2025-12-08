using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.ReportDefinitions.Get.v1;

public sealed record GetReportDefinitionRequest(DefaultIdType Id) : IRequest<ReportDefinitionResponse>;
