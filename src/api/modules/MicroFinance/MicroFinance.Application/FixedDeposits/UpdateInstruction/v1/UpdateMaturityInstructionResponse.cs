namespace FSH.Starter.WebApi.MicroFinance.Application.FixedDeposits.UpdateInstruction.v1;

/// <summary>
/// Response after updating instruction.
/// </summary>
public sealed record UpdateMaturityInstructionResponse(
    DefaultIdType DepositId,
    string MaturityInstruction,
    string Message);
