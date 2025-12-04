using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.FixedDeposits.UpdateInstruction.v1;

/// <summary>
/// Command to update maturity instruction.
/// </summary>
public sealed record UpdateMaturityInstructionCommand(Guid DepositId, string Instruction) : IRequest<UpdateMaturityInstructionResponse>;
