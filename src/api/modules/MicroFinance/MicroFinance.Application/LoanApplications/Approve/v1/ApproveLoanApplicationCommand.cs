using System.ComponentModel;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanApplications.Approve.v1;

/// <summary>
/// Command to approve a loan application.
/// </summary>
public sealed record ApproveLoanApplicationCommand(
    [property: DefaultValue("00000000-0000-0000-0000-000000000000")] DefaultIdType Id,
    [property: DefaultValue(95000)] decimal ApprovedAmount,
    [property: DefaultValue(12)] int ApprovedTermMonths,
    [property: DefaultValue(18.5)] decimal ApprovedInterestRate,
    [property: DefaultValue("00000000-0000-0000-0000-000000000000")] DefaultIdType ApproverId,
    [property: DefaultValue(null)] string? ApprovalConditions = null
) : IRequest<ApproveLoanApplicationResponse>;
