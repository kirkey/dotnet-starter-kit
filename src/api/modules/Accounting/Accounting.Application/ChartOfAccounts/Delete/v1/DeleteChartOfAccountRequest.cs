using FSH.Framework.Infrastructure.Abstract.Delete;

namespace Accounting.Application.ChartOfAccounts.Delete.v1;

public class DeleteChartOfAccountCommand(DefaultIdType id) : IRequest<DefaultIdType>
{
    public DefaultIdType Id { get; set; } = id;
}
