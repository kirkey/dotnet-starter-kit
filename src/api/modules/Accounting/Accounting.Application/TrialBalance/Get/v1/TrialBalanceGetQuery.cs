namespace Accounting.Application.TrialBalance.Get.v1;

/// <summary>
/// Query to retrieve a trial balance by ID.
/// </summary>
public sealed record TrialBalanceGetQuery(DefaultIdType Id) : IRequest<TrialBalanceGetResponse>;
