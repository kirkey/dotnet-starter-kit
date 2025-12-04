namespace FSH.Starter.WebApi.MicroFinance.Application.FixedDeposits.UpdateInstruction.v1;

/// <summary>
/// Response after updating instruction.
/// </summary>
public sealed record UpdateMaturityInstructionResponse(
    Guid DepositId,
    string MaturityInstruction,
    string Message);
