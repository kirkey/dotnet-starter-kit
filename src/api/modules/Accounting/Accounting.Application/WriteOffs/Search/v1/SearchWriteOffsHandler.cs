using Accounting.Application.WriteOffs.Queries;
using Accounting.Application.WriteOffs.Responses;

namespace Accounting.Application.WriteOffs.Search.v1;

/// <summary>
/// Handler for searching write-offs with filters and pagination.
/// </summary>
public sealed class SearchWriteOffsHandler(
    ILogger<SearchWriteOffsHandler> logger,
    [FromKeyedServices("accounting")] IReadRepository<WriteOff> repository)
    : IRequestHandler<SearchWriteOffsRequest, PagedList<WriteOffResponse>>
{
    public async Task<PagedList<WriteOffResponse>> Handle(SearchWriteOffsRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new WriteOffSearchSpec(request);
        
        var writeOffs = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Retrieved {Count} write-offs out of {Total}", writeOffs.Count, totalCount);

        var items = writeOffs.Select(w => new WriteOffResponse
        {
            Id = w.Id,
            ReferenceNumber = w.ReferenceNumber,
            WriteOffDate = w.WriteOffDate,
            CustomerId = w.CustomerId,
            CustomerName = w.CustomerName,
            Amount = w.Amount,
            WriteOffType = w.WriteOffType.ToString(),
            RecoveredAmount = w.RecoveredAmount,
            IsRecovered = w.IsRecovered
        }).ToList();

        return new PagedList<WriteOffResponse>(items, request.PageNumber, request.PageSize, totalCount);
    }
}
