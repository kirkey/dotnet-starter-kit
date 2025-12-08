namespace FSH.Starter.WebApi.MicroFinance.Application.UssdSessions.Timeout.v1;

/// <summary>
/// Response after timing out a USSD session.
/// </summary>
/// <param name="Id">The unique identifier of the timed out session.</param>
/// <param name="SessionId">The session ID string.</param>
/// <param name="Status">The new status of the session.</param>
/// <param name="EndedAt">The timestamp when the session ended.</param>
public sealed record TimeoutUssdSessionResponse(
    DefaultIdType Id,
    string SessionId,
    string Status,
    DateTimeOffset EndedAt);
