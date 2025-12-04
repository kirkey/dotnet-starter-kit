using System.ComponentModel;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.Loans.Disburse.v1;

public sealed record DisburseLoanCommand(
    Guid Id,
    [property: DefaultValue("BANK_TRANSFER")] string DisbursementMethod,
    [property: DefaultValue("TXN-2024-001")] string? TransactionReference,
    [property: DefaultValue("Disbursed to member's bank account")] string? Notes) : IRequest<DisburseLoanResponse>;
