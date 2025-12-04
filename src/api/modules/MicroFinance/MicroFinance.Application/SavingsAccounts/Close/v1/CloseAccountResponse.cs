namespace FSH.Starter.WebApi.MicroFinance.Application.SavingsAccounts.Close.v1;

/// <summary>
/// Response after closing account.
/// </summary>
public sealed record CloseAccountResponse(Guid AccountId, string Status, DateOnly? ClosedDate, string Message);
