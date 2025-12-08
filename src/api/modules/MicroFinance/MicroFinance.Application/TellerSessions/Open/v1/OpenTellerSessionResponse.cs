namespace FSH.Starter.WebApi.MicroFinance.Application.TellerSessions.Open.v1;

public sealed record OpenTellerSessionResponse(
    DefaultIdType Id,
    string SessionNumber,
    string TellerName,
    decimal OpeningBalance,
    string Status,
    DateTime StartTime);
