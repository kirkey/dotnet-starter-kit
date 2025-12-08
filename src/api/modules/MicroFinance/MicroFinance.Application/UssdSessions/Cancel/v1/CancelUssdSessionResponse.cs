namespace FSH.Starter.WebApi.MicroFinance.Application.UssdSessions.Cancel.v1;

/// <summary>
/// Response after cancelling a USSD session.
/// </summary>
/// <param name="Id">The unique identifier of the cancelled session.</param>
/// <param name="SessionId">The session ID string.</param>
/// <param name="Status">The new status of the session.</param>
/// <param name="EndedAt">The timestamp when the session ended.</param>
public sealed record CancelUssdSessionResponse(
    DefaultIdType Id,
    string SessionId,
    string Status,
    DateTimeOffset EndedAt);
