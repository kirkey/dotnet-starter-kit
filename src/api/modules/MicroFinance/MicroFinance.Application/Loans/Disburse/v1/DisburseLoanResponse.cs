namespace FSH.Starter.WebApi.MicroFinance.Application.Loans.Disburse.v1;

public sealed record DisburseLoanResponse(
    DefaultIdType Id,
    string Status,
    DateOnly DisbursementDate,
    DateOnly ExpectedEndDate,
    int TotalInstallments);
