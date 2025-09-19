using Accounting.Application.AccountingPeriods.Responses;

namespace Accounting.Application.AccountingPeriods.Get.v1;

/// <summary>
/// Query to retrieve a single AccountingPeriod by identifier.
/// </summary>
/// <remarks>
/// Used by the read-model / HTTP GET endpoint to fetch a detailed representation
/// of an accounting period for display or export.
/// </remarks>
public sealed class GetAccountingPeriodQuery(DefaultIdType id) : IRequest<AccountingPeriodResponse>
{
    /// <summary>
    /// The identifier of the accounting period to retrieve.
    /// </summary>
    public DefaultIdType Id { get; set; } = id;
}
