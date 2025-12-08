namespace FSH.Starter.WebApi.MicroFinance.Application.TellerSessions.Get.v1;

public sealed record TellerSessionResponse(
    DefaultIdType Id,
    DefaultIdType BranchId,
    DefaultIdType CashVaultId,
    string SessionNumber,
    DefaultIdType TellerUserId,
    string TellerName,
    DateOnly SessionDate,
    DateTime StartTime,
    DateTime? EndTime,
    decimal OpeningBalance,
    decimal TotalCashIn,
    decimal TotalCashOut,
    decimal ExpectedClosingBalance,
    decimal? ActualClosingBalance,
    decimal? Variance,
    int TransactionCount,
    string Status,
    DefaultIdType? SupervisorUserId,
    string? SupervisorName,
    DateTime? SupervisorVerificationTime);
