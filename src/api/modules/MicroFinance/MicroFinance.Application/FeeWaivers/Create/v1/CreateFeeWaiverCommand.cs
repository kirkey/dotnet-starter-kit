using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.FeeWaivers.Create.v1;

/// <summary>
/// Command to create a new fee waiver request.
/// </summary>
public sealed record CreateFeeWaiverCommand(
    DefaultIdType FeeChargeId,
    string Reference,
    decimal OriginalAmount,
    decimal WaivedAmount,
    string WaiverReason,
    DateOnly? RequestDate = null,
    string? Notes = null) : IRequest<CreateFeeWaiverResponse>;
