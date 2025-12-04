namespace FSH.Starter.WebApi.MicroFinance.Application.TellerSessions.Get.v1;

public sealed record TellerSessionResponse(
    Guid Id,
    Guid BranchId,
    Guid CashVaultId,
    string SessionNumber,
    Guid TellerUserId,
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
    Guid? SupervisorUserId,
    string? SupervisorName,
    DateTime? SupervisorVerificationTime);
