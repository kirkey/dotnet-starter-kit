using FSH.Starter.WebApi.Store.Application.Suppliers.Get.v1;

namespace FSH.Starter.WebApi.Store.Application.Suppliers.Specs;

/// <summary>
/// Specification to load a supplier by id projected to <see cref="SupplierResponse"/>.
/// </summary>
public class GetSupplierSpecs : Specification<Supplier, SupplierResponse>
{
    public GetSupplierSpecs(DefaultIdType id)
    {
        Query
            .Where(s => s.Id == id);
    }
}
