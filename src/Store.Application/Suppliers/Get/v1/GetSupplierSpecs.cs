namespace FSH.Starter.WebApi.Store.Application.Suppliers.Get.v1;

public class GetSupplierSpecs : Specification<Supplier, SupplierResponse>
{
    public GetSupplierSpecs(DefaultIdType id)
    {
        Query
            .Where(s => s.Id == id);
    }
}

