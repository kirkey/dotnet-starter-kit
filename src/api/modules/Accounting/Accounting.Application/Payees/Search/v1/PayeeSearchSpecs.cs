using Accounting.Application.Payees.Get.v1;

namespace Accounting.Application.Payees.Search.v1;
public class PayeeSearchSpecs : EntitiesByPaginationFilterSpec<Payee, PayeeResponse>
{
    public PayeeSearchSpecs(PayeeSearchCommand command)
        : base(command) =>
        Query
            .OrderBy(c => c.Name, !command.HasOrderBy());
}
