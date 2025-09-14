namespace FSH.Starter.WebApi.Store.Application.PurchaseOrders.Specs;

public class GetPurchaseOrderListSpecification : Specification<PurchaseOrder, FSH.Starter.WebApi.Store.Application.PurchaseOrders.Search.v1.GetPurchaseOrderListResponse>
{
    public GetPurchaseOrderListSpecification(FSH.Starter.WebApi.Store.Application.PurchaseOrders.Search.v1.SearchPurchaseOrdersCommand request)
    {
        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            Query.Where(po => po.OrderNumber.Contains(request.SearchTerm) || (po.DeliveryAddress != null && po.DeliveryAddress.Contains(request.SearchTerm)) || (po.ContactPerson != null && po.ContactPerson.Contains(request.SearchTerm)));
        }

        if (request.SupplierId.HasValue)
            Query.Where(po => po.SupplierId == request.SupplierId.Value);

        if (!string.IsNullOrWhiteSpace(request.Status))
            Query.Where(po => po.Status == request.Status);

        if (request.FromDate.HasValue)
            Query.Where(po => po.OrderDate >= request.FromDate.Value);

        if (request.ToDate.HasValue)
            Query.Where(po => po.OrderDate <= request.ToDate.Value);

        Query.Select(po => new FSH.Starter.WebApi.Store.Application.PurchaseOrders.Search.v1.GetPurchaseOrderListResponse(
            po.Id,
            po.OrderNumber,
            po.SupplierId,
            po.OrderDate,
            po.Status,
            po.TotalAmount));

        Query.OrderByDescending(po => po.OrderDate).ThenBy(po => po.OrderNumber);
    }
}

