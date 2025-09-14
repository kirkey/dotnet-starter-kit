namespace FSH.Starter.WebApi.Store.Application.PriceLists.Search.v1;

public record SearchPriceListsCommand(
    int PageNumber = 1,
    int PageSize = 10,
    string? SearchTerm = null,
    bool? IsActive = null,
    DateTime? FromDate = null,
    DateTime? ToDate = null
) : IRequest<PagedList<GetPriceListListResponse>>;

