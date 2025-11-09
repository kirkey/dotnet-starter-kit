namespace Accounting.Application.GeneralLedgers.Get.v1;

/// <summary>
/// Request to retrieve a general ledger entry by ID.
/// </summary>
public sealed record GeneralLedgerGetRequest(DefaultIdType Id) : IRequest<GeneralLedgerGetResponse>;
