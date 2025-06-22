using Ardalis.Specification;
using FSH.Starter.WebApi.Catalog.Domain;

namespace FSH.Starter.WebApi.Catalog.Application.Products.Get.v1;

public class GetProductSpecs : Specification<Product, ProductResponse>
{
    public GetProductSpecs(DefaultIdType id)
    {
        Query
            .Where(p => p.Id == id)
            .Include(p => p.Brand);
    }
}
