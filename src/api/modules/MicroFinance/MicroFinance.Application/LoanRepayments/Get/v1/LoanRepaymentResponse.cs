namespace FSH.Starter.WebApi.MicroFinance.Application.LoanRepayments.Get.v1;

/// <summary>
/// Response containing loan repayment details.
/// </summary>
/// <param name="Id">The unique identifier of the loan repayment.</param>
/// <param name="LoanId">The loan ID this repayment belongs to.</param>
/// <param name="LoanAccountNumber">The loan account number.</param>
/// <param name="MemberName">The member's full name.</param>
/// <param name="ReceiptNumber">The receipt number.</param>
/// <param name="RepaymentDate">The repayment date.</param>
/// <param name="PrincipalAmount">The principal amount paid.</param>
/// <param name="InterestAmount">The interest amount paid.</param>
/// <param name="PenaltyAmount">The penalty amount paid.</param>
/// <param name="TotalAmount">The total amount paid.</param>
/// <param name="PaymentMethod">The payment method.</param>
/// <param name="Notes">Internal notes.</param>
public record LoanRepaymentResponse(
    Guid Id,
    Guid LoanId,
    string? LoanAccountNumber,
    string? MemberName,
    string ReceiptNumber,
    DateOnly RepaymentDate,
    decimal PrincipalAmount,
    decimal InterestAmount,
    decimal PenaltyAmount,
    decimal TotalAmount,
    string PaymentMethod,
    string? Notes);
