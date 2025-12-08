namespace FSH.Starter.WebApi.MicroFinance.Application.TellerSessions.Verify.v1;

public sealed record VerifyTellerSessionResponse(
    DefaultIdType Id,
    string SupervisorName,
    DateTime VerificationTime);
