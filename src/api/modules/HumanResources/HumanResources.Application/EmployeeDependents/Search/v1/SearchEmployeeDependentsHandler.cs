using FSH.Starter.WebApi.HumanResources.Application.EmployeeDependents.Get.v1;
using FSH.Starter.WebApi.HumanResources.Application.EmployeeDependents.Specifications;

namespace FSH.Starter.WebApi.HumanResources.Application.EmployeeDependents.Search.v1;

/// <summary>
/// Handler for searching employee dependents.
/// </summary>
public sealed class SearchEmployeeDependentsHandler(
    [FromKeyedServices("hr:dependents")] IReadRepository<EmployeeDependent> repository)
    : IRequestHandler<SearchEmployeeDependentsRequest, PagedList<EmployeeDependentResponse>>
{
    public async Task<PagedList<EmployeeDependentResponse>> Handle(
        SearchEmployeeDependentsRequest request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchEmployeeDependentsSpec(request);

        var items = await repository
            .ListAsync(spec, cancellationToken)
            .ConfigureAwait(false);

        var totalCount = await repository
            .CountAsync(spec, cancellationToken)
            .ConfigureAwait(false);

        return new PagedList<EmployeeDependentResponse>(items, request.PageNumber, request.PageSize, totalCount);
    }
}

