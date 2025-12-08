using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanGuarantors.UpdateAmount.v1;

/// <summary>
/// Command to update the guaranteed amount.
/// </summary>
public sealed record UpdateGuaranteedAmountCommand(DefaultIdType Id, decimal GuaranteedAmount) : IRequest<UpdateGuaranteedAmountResponse>;
