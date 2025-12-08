namespace FSH.Starter.WebApi.MicroFinance.Application.CreditBureauInquiries.Get.v1;

public sealed record CreditBureauInquiryResponse(
    DefaultIdType Id,
    DefaultIdType MemberId,
    DefaultIdType? LoanId,
    string InquiryNumber,
    string BureauName,
    string Purpose,
    DateTime InquiryDate,
    string? RequestedBy,
    DefaultIdType? RequestedByUserId,
    string? ReferenceNumber,
    string Status,
    DateTime? ResponseReceivedAt,
    int? CreditScore,
    DefaultIdType? CreditReportId,
    decimal? InquiryCost,
    string? ErrorMessage,
    string? Notes);
