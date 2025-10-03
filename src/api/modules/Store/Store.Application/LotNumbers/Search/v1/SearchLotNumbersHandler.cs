namespace FSH.Starter.WebApi.Store.Application.LotNumbers.Search.v1;

/// <summary>
/// Handler for searching lot numbers.
/// </summary>
public sealed class SearchLotNumbersHandler(
    [FromKeyedServices("store:lotnumbers")] IReadRepository<LotNumber> repository)
    : IRequestHandler<SearchLotNumbersCommand, PagedList<LotNumberResponse>>
{
    public async Task<PagedList<LotNumberResponse>> Handle(SearchLotNumbersCommand request, CancellationToken cancellationToken)
    {
        var spec = new Specs.SearchLotNumbersSpec(request);

        var lotNumbers = await repository.ListAsync(spec, cancellationToken);
        var totalCount = await repository.CountAsync(spec, cancellationToken);

        var lotNumberResponses = lotNumbers.Select(lotNumber => new LotNumberResponse
        {
            Id = lotNumber.Id,
            LotNumber = lotNumber.LotNumberValue,
            ItemId = lotNumber.ItemId,
            ExpirationDate = lotNumber.ExpirationDate,
            QuantityOnHand = lotNumber.QuantityOnHand,
            Status = lotNumber.Status,
            ManufacturedDate = lotNumber.ManufacturedDate,
            ReceivedDate = lotNumber.ReceivedDate
        }).ToList();

        return new PagedList<LotNumberResponse>(lotNumberResponses, request.PageNumber, request.PageSize, totalCount);
    }
}
