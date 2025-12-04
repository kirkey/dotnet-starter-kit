using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.CreditBureauReports.Get.v1;

public sealed record GetCreditBureauReportRequest(Guid Id) : IRequest<CreditBureauReportResponse>;
