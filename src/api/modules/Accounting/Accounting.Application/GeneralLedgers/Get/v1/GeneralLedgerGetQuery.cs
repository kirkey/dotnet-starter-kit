namespace Accounting.Application.GeneralLedgers.Get.v1;

/// <summary>
/// Query to retrieve a general ledger entry by ID.
/// </summary>
public sealed record GeneralLedgerGetQuery(DefaultIdType Id) : IRequest<GeneralLedgerGetResponse>;
