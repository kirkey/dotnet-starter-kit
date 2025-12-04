using System.ComponentModel;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.Loans.Approve.v1;

public sealed record ApproveLoanCommand(
    Guid Id,
    [property: DefaultValue("Loan approved after credit assessment")] string? Notes) : IRequest<ApproveLoanResponse>;
