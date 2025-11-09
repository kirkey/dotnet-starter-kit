namespace Accounting.Application.TrialBalance.Get.v1;

/// <summary>
/// Request to retrieve a trial balance by ID.
/// </summary>
public sealed record TrialBalanceGetRequest(DefaultIdType Id) : IRequest<TrialBalanceGetResponse>;
