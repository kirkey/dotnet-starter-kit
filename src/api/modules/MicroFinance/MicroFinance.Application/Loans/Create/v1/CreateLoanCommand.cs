using System.ComponentModel;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.Loans.Create.v1;

public sealed record CreateLoanCommand(
    [property: DefaultValue("00000000-0000-0000-0000-000000000000")] Guid MemberId,
    [property: DefaultValue("00000000-0000-0000-0000-000000000000")] Guid LoanProductId,
    [property: DefaultValue(50000)] decimal RequestedAmount,
    [property: DefaultValue(12)] int TermMonths,
    [property: DefaultValue("Business expansion loan")] string? Purpose,
    [property: DefaultValue("MONTHLY")] string RepaymentFrequency) : IRequest<CreateLoanResponse>;
