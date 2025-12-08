namespace FSH.Starter.WebApi.MicroFinance.Application.UssdSessions.Complete.v1;

/// <summary>
/// Response after completing a USSD session.
/// </summary>
/// <param name="Id">The unique identifier of the completed session.</param>
/// <param name="SessionId">The session ID string.</param>
/// <param name="Status">The new status of the session.</param>
/// <param name="StepCount">Total number of steps in the session.</param>
/// <param name="EndedAt">The timestamp when the session ended.</param>
public sealed record CompleteUssdSessionResponse(
    DefaultIdType Id,
    string SessionId,
    string Status,
    int StepCount,
    DateTimeOffset EndedAt);
