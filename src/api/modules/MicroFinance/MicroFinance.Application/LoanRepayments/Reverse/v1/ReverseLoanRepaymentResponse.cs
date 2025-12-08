namespace FSH.Starter.WebApi.MicroFinance.Application.LoanRepayments.Reverse.v1;

/// <summary>
/// Response after reversing a loan repayment.
/// </summary>
public sealed record ReverseLoanRepaymentResponse(DefaultIdType LoanRepaymentId, string Status, string Message);
