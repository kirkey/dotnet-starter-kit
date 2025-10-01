using FSH.Starter.WebApi.Store.Application.SalesOrders.Search.v1;

namespace FSH.Starter.WebApi.Store.Application.SalesOrders.Specs;

public class SearchSalesOrdersSpecs : EntitiesByPaginationFilterSpec<SalesOrder, Get.v1.GetSalesOrderResponse>
{
    public SearchSalesOrdersSpecs(SearchSalesOrdersCommand command)
        : base(command)
    {
        Query
            .OrderByDescending(so => so.OrderDate, !command.HasOrderBy())
            .Where(so => so.OrderNumber.Contains(command.OrderNumber!), !string.IsNullOrEmpty(command.OrderNumber))
            .Where(so => so.CustomerId == command.CustomerId!.Value, command.CustomerId.HasValue)
            .Where(so => so.Status == command.Status, !string.IsNullOrEmpty(command.Status))
            .Where(so => so.OrderDate >= command.FromDate!.Value, command.FromDate.HasValue)
            .Where(so => so.OrderDate <= command.ToDate!.Value, command.ToDate.HasValue)
            .Where(so => so.IsUrgent == command.IsUrgent, command.IsUrgent.HasValue)
            .Where(so => so.WarehouseId == command.WarehouseId!.Value, command.WarehouseId.HasValue);
    }
}

