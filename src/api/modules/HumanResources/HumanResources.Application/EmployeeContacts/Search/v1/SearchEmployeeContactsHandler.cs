using FSH.Starter.WebApi.HumanResources.Application.EmployeeContacts.Get.v1;
using FSH.Starter.WebApi.HumanResources.Application.EmployeeContacts.Specifications;

namespace FSH.Starter.WebApi.HumanResources.Application.EmployeeContacts.Search.v1;

/// <summary>
/// Handler for searching employee contacts.
/// </summary>
public sealed class SearchEmployeeContactsHandler(
    [FromKeyedServices("hr:contacts")] IReadRepository<EmployeeContact> repository)
    : IRequestHandler<SearchEmployeeContactsRequest, PagedList<EmployeeContactResponse>>
{
    public async Task<PagedList<EmployeeContactResponse>> Handle(
        SearchEmployeeContactsRequest request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchEmployeeContactsSpec(request);

        var items = await repository
            .ListAsync(spec, cancellationToken)
            .ConfigureAwait(false);

        var totalCount = await repository
            .CountAsync(spec, cancellationToken)
            .ConfigureAwait(false);

        return new PagedList<EmployeeContactResponse>(items, request.PageNumber, request.PageSize, totalCount);
    }
}

