using FSH.Starter.WebApi.Store.Application.SerialNumbers.Specs;

namespace FSH.Starter.WebApi.Store.Application.SerialNumbers.Search.v1;

public class SearchSerialNumbersHandler(
    [FromKeyedServices("store:serialnumbers")] IReadRepository<SerialNumber> repository)
    : IRequestHandler<SearchSerialNumbersCommand, PagedList<SerialNumberResponse>>
{
    public async Task<PagedList<SerialNumberResponse>> Handle(SearchSerialNumbersCommand request, CancellationToken cancellationToken)
    {
        var spec = new SearchSerialNumbersSpec(request);

        var serialNumbers = await repository.ListAsync(spec, cancellationToken);
        var totalCount = await repository.CountAsync(spec, cancellationToken);

        return new PagedList<SerialNumberDto>(serialNumbers, request.PageNumber, request.PageSize, totalCount);
    }
}
