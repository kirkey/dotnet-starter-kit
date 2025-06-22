using Accounting.Application.Payees.Get.v1;
using FSH.Framework.Core.Paging;
using MediatR;

namespace Accounting.Application.Payees.Search.v1;

public class PayeeSearchCommand : PaginationFilter, IRequest<PagedList<PayeeResponse>>
{
    // public string? Name { get; set; }
}
