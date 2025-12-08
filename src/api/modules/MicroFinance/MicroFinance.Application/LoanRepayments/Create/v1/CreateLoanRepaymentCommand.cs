using System.ComponentModel;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanRepayments.Create.v1;

public sealed record CreateLoanRepaymentCommand(
    [property: DefaultValue("00000000-0000-0000-0000-000000000000")] DefaultIdType LoanId,
    [property: DefaultValue(5000)] decimal Amount,
    [property: DefaultValue("CASH")] string PaymentMethod,
    [property: DefaultValue("TXN-2024-001")] string? TransactionReference,
    [property: DefaultValue("Monthly installment payment")] string? Notes) : IRequest<CreateLoanRepaymentResponse>;
