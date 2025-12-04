using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.FeeCharges.Create.v1;

/// <summary>
/// Command to create a new fee charge.
/// </summary>
public sealed record CreateFeeChargeCommand(
    Guid FeeDefinitionId,
    Guid MemberId,
    string Reference,
    decimal Amount,
    Guid? LoanId = null,
    Guid? SavingsAccountId = null,
    Guid? ShareAccountId = null,
    DateOnly? ChargeDate = null,
    DateOnly? DueDate = null,
    string? Notes = null) : IRequest<CreateFeeChargeResponse>;
