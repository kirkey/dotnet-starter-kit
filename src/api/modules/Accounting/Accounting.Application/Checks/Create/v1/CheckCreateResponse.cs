namespace Accounting.Application.Checks.Create.v1;

/// <summary>
/// Response after successfully creating a bundle of checks.
/// </summary>
public record CheckCreateResponse(
    DefaultIdType Id,
    string StartCheckNumber,
    string EndCheckNumber,
    int ChecksCreated);


