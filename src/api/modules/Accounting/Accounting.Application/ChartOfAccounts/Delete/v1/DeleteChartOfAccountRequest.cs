using FSH.Framework.Infrastructure.Abstract.Delete;

namespace Accounting.Application.ChartOfAccounts.Delete.v1;

public class DeleteChartOfAccountRequest : DeleteRequest<DefaultIdType>
{
    public DeleteChartOfAccountRequest(DefaultIdType id) : base(id) {}
}
