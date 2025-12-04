namespace FSH.Starter.WebApi.MicroFinance.Application.SavingsAccounts.Get.v1;

/// <summary>
/// Response DTO for a savings account.
/// </summary>
public sealed record SavingsAccountResponse(
    Guid Id,
    string AccountNumber,
    Guid MemberId,
    string? MemberName,
    Guid SavingsProductId,
    string? ProductName,
    decimal Balance,
    decimal TotalDeposits,
    decimal TotalWithdrawals,
    decimal TotalInterestEarned,
    DateOnly OpenedDate,
    DateOnly? ClosedDate,
    DateOnly? LastInterestPostingDate,
    string Status,
    string? Notes);
