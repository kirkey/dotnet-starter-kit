using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.MicroFinance.Application.StaffTrainings.Search.v1;

public sealed class SearchStaffTrainingsHandler(
    [FromKeyedServices("microfinance:stafftrainings")] IReadRepository<StaffTraining> repository)
    : IRequestHandler<SearchStaffTrainingsCommand, PagedList<StaffTrainingSummaryResponse>>
{
    public async Task<PagedList<StaffTrainingSummaryResponse>> Handle(
        SearchStaffTrainingsCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchStaffTrainingsSpecs(request);

        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<StaffTrainingSummaryResponse>(items, request.PageNumber, request.PageSize, totalCount);
    }
}
