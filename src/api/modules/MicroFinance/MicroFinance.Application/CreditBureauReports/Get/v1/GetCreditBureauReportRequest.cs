using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.CreditBureauReports.Get.v1;

public sealed record GetCreditBureauReportRequest(DefaultIdType Id) : IRequest<CreditBureauReportResponse>;
