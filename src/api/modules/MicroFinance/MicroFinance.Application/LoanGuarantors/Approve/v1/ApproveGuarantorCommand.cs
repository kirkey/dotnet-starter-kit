using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanGuarantors.Approve.v1;

/// <summary>
/// Command to approve a loan guarantor.
/// </summary>
public sealed record ApproveGuarantorCommand(DefaultIdType Id) : IRequest<ApproveGuarantorResponse>;
