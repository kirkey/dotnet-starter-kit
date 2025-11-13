using FSH.Starter.WebApi.HumanResources.Application.Attendance.Get.v1;
using FSH.Starter.WebApi.HumanResources.Application.Attendance.Specifications;

namespace FSH.Starter.WebApi.HumanResources.Application.Attendance.Search.v1;

public sealed class SearchAttendanceHandler(
    [FromKeyedServices("hr:attendance")] IReadRepository<Domain.Entities.Attendance> repository)
    : IRequestHandler<SearchAttendanceRequest, PagedList<AttendanceResponse>>
{
    public async Task<PagedList<AttendanceResponse>> Handle(
        SearchAttendanceRequest request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchAttendanceSpec(request);

        var items = await repository
            .ListAsync(spec, cancellationToken)
            .ConfigureAwait(false);

        var totalCount = await repository
            .CountAsync(spec, cancellationToken)
            .ConfigureAwait(false);

        return new PagedList<AttendanceResponse>(items, request.PageNumber, request.PageSize, totalCount);
    }
}

