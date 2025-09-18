using Accounting.Application.DeferredRevenues.Dtos;
using Accounting.Application.DeferredRevenues.Queries;

namespace Accounting.Application.DeferredRevenues.Handlers;

public class GetDeferredRevenueByIdHandler(IReadRepository<DeferredRevenue> repository)
    : IRequestHandler<GetDeferredRevenueByIdQuery, DeferredRevenueDto>
{
    public async Task<DeferredRevenueDto> Handle(GetDeferredRevenueByIdQuery request, CancellationToken cancellationToken)
    {
        var deferred = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (deferred == null)
            throw new NotFoundException($"DeferredRevenue with Id {request.Id} not found");
        return new DeferredRevenueDto
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