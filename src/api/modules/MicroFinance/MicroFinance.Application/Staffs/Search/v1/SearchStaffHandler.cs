// filepath: /Users/kirkeypsalms/Projects/dotnet-starter-kit/src/api/modules/MicroFinance/MicroFinance.Application/Staff/Search/v1/SearchStaffHandler.cs

using FSH.Starter.WebApi.MicroFinance.Application.Staffs.Get.v1;

namespace FSH.Starter.WebApi.MicroFinance.Application.Staffs.Search.v1;

/// <summary>
/// Handler for searching staff members.
/// </summary>
public sealed class SearchStaffHandler(
    [FromKeyedServices("microfinance:staff")] IReadRepository<Domain.Staff> repository)
    : IRequestHandler<SearchStaffCommand, PagedList<StaffResponse>>
{
    public async Task<PagedList<StaffResponse>> Handle(SearchStaffCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchStaffSpecs(request);

        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<StaffResponse>(items, request.PageNumber, request.PageSize, totalCount);
    }
}

