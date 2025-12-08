using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.ReportGenerations.Get.v1;

public sealed record GetReportGenerationRequest(DefaultIdType Id) : IRequest<ReportGenerationResponse>;
