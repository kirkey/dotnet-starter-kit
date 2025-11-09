using Accounting.Application.DeferredRevenues.Responses;

namespace Accounting.Application.DeferredRevenues.Get;

public sealed class GetDeferredRevenueHandler(IRepository<DeferredRevenue> repository)
    : IRequestHandler<GetDeferredRevenueRequest, DeferredRevenueResponse>
{
    private readonly IRepository<DeferredRevenue> _repository = repository ?? throw new ArgumentNullException(nameof(repository));

    public async Task<DeferredRevenueResponse> Handle(GetDeferredRevenueRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var deferredRevenue = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (deferredRevenue == null)
            throw new DeferredRevenueByIdNotFoundException(request.Id);

        return new DeferredRevenueResponse
        {
            Id = deferredRevenue.Id,
            DeferredRevenueNumber = deferredRevenue.DeferredRevenueNumber,
            RecognitionDate = deferredRevenue.RecognitionDate,
            Amount = deferredRevenue.Amount,
            Description = deferredRevenue.Description,
            IsRecognized = deferredRevenue.IsRecognized,
            RecognizedDate = deferredRevenue.RecognizedDate
        };
    }
}
