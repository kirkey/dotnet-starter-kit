using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.CreditBureauReports.Create.v1;

public sealed record CreateCreditBureauReportCommand(
    DefaultIdType MemberId,
    string ReportNumber,
    string BureauName,
    DateTime ReportDate,
    DefaultIdType? InquiryId = null,
    int? CreditScore = null,
    int? ScoreMin = null,
    int? ScoreMax = null,
    string? ScoreModel = null,
    DateTime? ExpiryDate = null) : IRequest<CreateCreditBureauReportResponse>;
