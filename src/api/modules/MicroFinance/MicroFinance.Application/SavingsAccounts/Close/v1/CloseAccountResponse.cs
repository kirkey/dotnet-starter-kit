namespace FSH.Starter.WebApi.MicroFinance.Application.SavingsAccounts.Close.v1;

/// <summary>
/// Response after closing account.
/// </summary>
public sealed record CloseAccountResponse(DefaultIdType AccountId, string Status, DateOnly? ClosedDate, string Message);
