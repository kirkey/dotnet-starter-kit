namespace FSH.Starter.WebApi.MicroFinance.Application.CreditBureauInquiries.Get.v1;

public sealed record CreditBureauInquiryResponse(
    Guid Id,
    Guid MemberId,
    Guid? LoanId,
    string InquiryNumber,
    string BureauName,
    string Purpose,
    DateTime InquiryDate,
    string? RequestedBy,
    Guid? RequestedByUserId,
    string? ReferenceNumber,
    string Status,
    DateTime? ResponseReceivedAt,
    int? CreditScore,
    Guid? CreditReportId,
    decimal? InquiryCost,
    string? ErrorMessage,
    string? Notes);
