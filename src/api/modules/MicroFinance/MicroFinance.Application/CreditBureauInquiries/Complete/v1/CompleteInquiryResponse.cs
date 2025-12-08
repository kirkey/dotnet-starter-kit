namespace FSH.Starter.WebApi.MicroFinance.Application.CreditBureauInquiries.Complete.v1;

public sealed record CompleteInquiryResponse(DefaultIdType Id, string Status, int? CreditScore);
