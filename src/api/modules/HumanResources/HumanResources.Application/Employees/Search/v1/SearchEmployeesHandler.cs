using FSH.Starter.WebApi.HumanResources.Application.Employees.Get.v1;
using FSH.Starter.WebApi.HumanResources.Application.Employees.Specifications;

namespace FSH.Starter.WebApi.HumanResources.Application.Employees.Search.v1;

public sealed class SearchEmployeesHandler(
    [FromKeyedServices("hr:employees")] IReadRepository<Employee> repository)
    : IRequestHandler<SearchEmployeesRequest, PagedList<EmployeeResponse>>
{
    public async Task<PagedList<EmployeeResponse>> Handle(
        SearchEmployeesRequest request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchEmployeesSpec(request);

        var items = await repository
            .ListAsync(spec, cancellationToken)
            .ConfigureAwait(false);

        var totalCount = await repository
            .CountAsync(spec, cancellationToken)
            .ConfigureAwait(false);

        return new PagedList<EmployeeResponse>(items, request.PageNumber, request.PageSize, totalCount);
    }
}

