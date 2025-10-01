namespace FSH.Starter.WebApi.Store.Application.PriceLists.Specs;

public class GetPriceListListSpecification : Specification<PriceList, Search.v1.GetPriceListListResponse>
{
    public GetPriceListListSpecification(Search.v1.SearchPriceListsCommand request)
    {
        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            Query.Where(p => p.Name.Contains(request.SearchTerm) || p.PriceListName.Contains(request.SearchTerm) || (p.CustomerType != null && p.CustomerType.Contains(request.SearchTerm)) || p.Currency.Contains(request.SearchTerm));
        }

        if (request.IsActive.HasValue)
        {
            Query.Where(p => p.IsActive == request.IsActive.Value);
        }

        if (request.FromDate.HasValue)
        {
            Query.Where(p => p.EffectiveDate >= request.FromDate.Value);
        }

        if (request.ToDate.HasValue)
        {
            Query.Where(p => p.EffectiveDate <= request.ToDate.Value);
        }

        Query.Select(p => new Search.v1.GetPriceListListResponse(
            p.Id,
            p.Name,
            p.PriceListName,
            p.EffectiveDate,
            p.ExpiryDate,
            p.IsActive,
            p.Currency));

        Query.OrderByDescending(p => p.EffectiveDate).ThenBy(p => p.Name);
    }
}

