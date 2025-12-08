namespace FSH.Starter.WebApi.MicroFinance.Application.ShareAccounts.RedeemShares.v1;

/// <summary>
/// Response after redeeming shares.
/// </summary>
public sealed record RedeemSharesResponse(
    DefaultIdType AccountId,
    int SharesRedeemed,
    decimal TotalAmount,
    int RemainingShares,
    string Message);
