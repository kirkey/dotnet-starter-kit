namespace Accounting.Application.Checks.Get.v1;

/// <summary>
/// Query to get a single check by ID.
/// </summary>
public record CheckGetQuery(DefaultIdType CheckId) : IRequest<CheckGetResponse>;
