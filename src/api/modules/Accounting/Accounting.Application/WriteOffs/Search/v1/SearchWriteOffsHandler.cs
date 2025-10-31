using Accounting.Application.WriteOffs.Queries;
using Accounting.Application.WriteOffs.Responses;

namespace Accounting.Application.WriteOffs.Search.v1;

/// <summary>
/// Handler for searching write-offs with filters.
/// </summary>
public sealed class SearchWriteOffsHandler(
    ILogger<SearchWriteOffsHandler> logger,
    [FromKeyedServices("accounting")] IReadRepository<WriteOff> repository)
    : IRequestHandler<SearchWriteOffsRequest, List<WriteOffResponse>>
{
    public async Task<List<WriteOffResponse>> Handle(SearchWriteOffsRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new WriteOffSearchSpec(
            request.ReferenceNumber,
            request.CustomerId,
            request.WriteOffType,
            request.Status,
            isRecovered: request.IsRecovered);

        var writeOffs = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Retrieved {Count} write-offs", writeOffs.Count);

        return writeOffs.Select(w => new WriteOffResponse
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
    }
}
