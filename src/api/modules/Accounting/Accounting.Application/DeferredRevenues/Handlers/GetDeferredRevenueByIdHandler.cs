using Accounting.Application.DeferredRevenues.Responses;
using Accounting.Application.DeferredRevenues.Queries;

namespace Accounting.Application.DeferredRevenues.Handlers;

public class GetDeferredRevenueByIdHandler(IReadRepository<DeferredRevenue> repository)
    : IRequestHandler<GetDeferredRevenueByIdQuery, DeferredRevenueResponse>
{
    public async Task<DeferredRevenueResponse> Handle(GetDeferredRevenueByIdQuery request, CancellationToken cancellationToken)
    {
        var deferred = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (deferred == null)
            throw new NotFoundException($"DeferredRevenue with Id {request.Id} not found");
        return new DeferredRevenueResponse
        {
            Id = deferred.Id,
            DeferredRevenueNumber = deferred.DeferredRevenueNumber,
            RecognitionDate = deferred.RecognitionDate,
            Amount = deferred.Amount,
            Description = deferred.Description,
            IsRecognized = deferred.IsRecognized,
            RecognizedDate = deferred.RecognizedDate
        };
    }
}
