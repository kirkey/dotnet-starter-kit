namespace FSH.Starter.WebApi.MicroFinance.Application.ShareAccounts.Close.v1;

/// <summary>
/// Response after closing share account.
/// </summary>
public sealed record CloseShareAccountResponse(
    DefaultIdType AccountId,
    string Status,
    DateOnly? ClosedDate,
    string Message);
