namespace FSH.Starter.WebApi.MicroFinance.Application.TellerSessions.RecordCashIn.v1;

public sealed record RecordCashInResponse(
    Guid Id,
    decimal Amount,
    decimal TotalCashIn,
    decimal ExpectedClosingBalance,
    int TransactionCount);
