using System.ComponentModel;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanApplications.Reject.v1;

/// <summary>
/// Command to reject a loan application.
/// </summary>
public sealed record RejectLoanApplicationCommand(
    [property: DefaultValue("00000000-0000-0000-0000-000000000000")] Guid Id,
    [property: DefaultValue("Insufficient credit history")] string Reason,
    [property: DefaultValue("00000000-0000-0000-0000-000000000000")] Guid RejectedById
) : IRequest<RejectLoanApplicationResponse>;
