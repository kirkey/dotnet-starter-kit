using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.ShareAccounts.RedeemShares.v1;

/// <summary>
/// Command to redeem (sell back) shares from an account.
/// </summary>
public sealed record RedeemSharesCommand(
    Guid ShareAccountId,
    int NumberOfShares,
    decimal PricePerShare) : IRequest<RedeemSharesResponse>;
