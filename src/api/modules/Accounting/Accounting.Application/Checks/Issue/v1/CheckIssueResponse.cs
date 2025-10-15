namespace Accounting.Application.Checks.Issue.v1;

/// <summary>
/// Response after successfully issuing a check.
/// </summary>
public record CheckIssueResponse(DefaultIdType Id, string CheckNumber, string Status);

