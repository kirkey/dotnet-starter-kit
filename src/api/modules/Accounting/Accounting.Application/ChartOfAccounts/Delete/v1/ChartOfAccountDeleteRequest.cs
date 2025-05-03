using FSH.Framework.Infrastructure.Abstract.Delete;

namespace Accounting.Application.ChartOfAccounts.Delete.v1;

public class ChartOfAccountDeleteRequest(DefaultIdType id) : DeleteRequest<DefaultIdType>(id);
