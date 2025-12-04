namespace FSH.Starter.WebApi.MicroFinance.Application.TellerSessions.RecordCashOut.v1;

public sealed record RecordCashOutResponse(
    Guid Id,
    decimal Amount,
    decimal TotalCashOut,
    decimal ExpectedClosingBalance,
    int TransactionCount);
