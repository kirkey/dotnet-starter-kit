namespace FSH.Starter.WebApi.MicroFinance.Application.TellerSessions.TransferCash.v1;

/// <summary>
/// Response after transferring cash.
/// </summary>
public sealed record TransferCashResponse(
    DefaultIdType TellerSessionId, 
    decimal Amount,
    bool IsTransferIn,
    decimal ExpectedBalance,
    string Message);
