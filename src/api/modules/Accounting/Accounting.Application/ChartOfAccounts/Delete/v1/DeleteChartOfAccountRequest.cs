using FSH.Framework.Infrastructure.Abstract.Delete;

namespace Accounting.Application.ChartOfAccounts.Delete.v1;

public class DeleteChartOfAccountRequest(DefaultIdType id) : DeleteRequest<DefaultIdType>(id);
