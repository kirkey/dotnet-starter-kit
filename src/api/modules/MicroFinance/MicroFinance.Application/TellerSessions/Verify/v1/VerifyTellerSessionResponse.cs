namespace FSH.Starter.WebApi.MicroFinance.Application.TellerSessions.Verify.v1;

public sealed record VerifyTellerSessionResponse(
    Guid Id,
    string SupervisorName,
    DateTime VerificationTime);
