using Accounting.Application.ChartOfAccounts.Responses;

namespace Accounting.Application.ChartOfAccounts.Get.v1;

/// <summary>
/// Request to retrieve a chart of account by ID.
/// </summary>
public class GetChartOfAccountRequest(DefaultIdType id) : IRequest<ChartOfAccountResponse>
{
    /// <summary>
    /// The ID of the chart of account to retrieve.
    /// </summary>
    public DefaultIdType Id { get; set; } = id;
}
