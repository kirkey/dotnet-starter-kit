namespace FSH.Starter.WebApi.MicroFinance.Application.Loans.Disburse.v1;

public sealed record DisburseLoanResponse(
    Guid Id,
    string Status,
    DateOnly DisbursementDate,
    DateOnly ExpectedEndDate,
    int TotalInstallments);
