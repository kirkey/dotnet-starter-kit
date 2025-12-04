using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanGuarantors.UpdateAmount.v1;

/// <summary>
/// Command to update the guaranteed amount.
/// </summary>
public sealed record UpdateGuaranteedAmountCommand(Guid Id, decimal GuaranteedAmount) : IRequest<UpdateGuaranteedAmountResponse>;
