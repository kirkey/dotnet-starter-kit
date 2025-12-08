namespace FSH.Starter.WebApi.MicroFinance.Application.ShareAccounts.PurchaseShares.v1;

public sealed record PurchaseSharesResponse(DefaultIdType Id, int TotalShares, decimal TotalShareValue);
