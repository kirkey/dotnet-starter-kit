using System.ComponentModel;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanApplications.Assign.v1;

/// <summary>
/// Command to assign a loan application to a loan officer.
/// </summary>
public sealed record AssignLoanApplicationCommand(
    [property: DefaultValue("00000000-0000-0000-0000-000000000000")] DefaultIdType Id,
    [property: DefaultValue("00000000-0000-0000-0000-000000000000")] DefaultIdType OfficerId
) : IRequest<AssignLoanApplicationResponse>;
