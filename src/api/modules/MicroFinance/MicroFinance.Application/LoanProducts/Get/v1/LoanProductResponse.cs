namespace FSH.Starter.WebApi.MicroFinance.Application.LoanProducts.Get.v1;

public sealed record LoanProductResponse(
    DefaultIdType Id,
    string Code,
    string Name,
    string? Description,
    decimal MinLoanAmount,
    decimal MaxLoanAmount,
    decimal InterestRate,
    string InterestMethod,
    int MinTermMonths,
    int MaxTermMonths,
    string RepaymentFrequency,
    int GracePeriodDays,
    decimal LatePenaltyRate,
    bool IsActive
);
