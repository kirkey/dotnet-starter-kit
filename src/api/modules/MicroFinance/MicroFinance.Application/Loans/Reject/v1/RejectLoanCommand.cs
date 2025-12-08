using System.ComponentModel;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.Loans.Reject.v1;

public sealed record RejectLoanCommand(
    DefaultIdType Id,
    [property: DefaultValue("Insufficient credit score")] string RejectionReason) : IRequest<RejectLoanResponse>;
