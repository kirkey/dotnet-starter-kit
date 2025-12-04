namespace FSH.Starter.WebApi.MicroFinance.Application.ShareAccounts.PostDividend.v1;

/// <summary>
/// Response after posting dividend.
/// </summary>
public sealed record PostDividendResponse(
    Guid AccountId,
    decimal DividendAmount,
    decimal TotalDividendsEarned,
    string Message);
