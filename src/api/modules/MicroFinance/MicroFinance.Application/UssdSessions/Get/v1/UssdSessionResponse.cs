namespace FSH.Starter.WebApi.MicroFinance.Application.UssdSessions.Get.v1;

public sealed record UssdSessionResponse(
    Guid Id,
    string SessionId,
    string PhoneNumber,
    string ServiceCode,
    Guid? MemberId,
    Guid? WalletId,
    string Status,
    string CurrentMenu,
    string? Language,
    string? CurrentOperation,
    string? SessionData,
    int MenuLevel,
    int StepCount,
    DateTimeOffset StartedAt,
    DateTimeOffset? EndedAt,
    DateTimeOffset LastActivityAt,
    int SessionTimeoutSeconds,
    string? LastInput,
    string? LastOutput,
    bool IsAuthenticated,
    string? ErrorMessage);
