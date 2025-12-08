using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.FeeCharges.Create.v1;

/// <summary>
/// Command to create a new fee charge.
/// </summary>
public sealed record CreateFeeChargeCommand(
    DefaultIdType FeeDefinitionId,
    DefaultIdType MemberId,
    string Reference,
    decimal Amount,
    DefaultIdType? LoanId = null,
    DefaultIdType? SavingsAccountId = null,
    DefaultIdType? ShareAccountId = null,
    DateOnly? ChargeDate = null,
    DateOnly? DueDate = null,
    string? Notes = null) : IRequest<CreateFeeChargeResponse>;
