namespace FSH.Starter.WebApi.MicroFinance.Application.ShareAccounts.PayDividend.v1;

/// <summary>
/// Response after paying dividend.
/// </summary>
public sealed record PayDividendResponse(
    DefaultIdType AccountId,
    decimal AmountPaid,
    decimal TotalDividendsPaid,
    string Message);
