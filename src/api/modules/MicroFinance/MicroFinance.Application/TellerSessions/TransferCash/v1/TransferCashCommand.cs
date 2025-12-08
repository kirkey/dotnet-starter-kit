using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.TellerSessions.TransferCash.v1;

/// <summary>
/// Command to transfer cash to/from a teller session (vault to teller or teller to vault).
/// </summary>
public sealed record TransferCashCommand(
    DefaultIdType TellerSessionId, 
    decimal Amount,
    bool IsTransferIn,
    string Reference) : IRequest<TransferCashResponse>;
