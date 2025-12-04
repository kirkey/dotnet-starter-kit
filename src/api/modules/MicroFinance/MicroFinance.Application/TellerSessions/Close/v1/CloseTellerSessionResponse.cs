namespace FSH.Starter.WebApi.MicroFinance.Application.TellerSessions.Close.v1;

public sealed record CloseTellerSessionResponse(
    Guid Id,
    string Status,
    decimal ExpectedClosingBalance,
    decimal ActualClosingBalance,
    decimal Variance,
    DateTime? EndTime);
