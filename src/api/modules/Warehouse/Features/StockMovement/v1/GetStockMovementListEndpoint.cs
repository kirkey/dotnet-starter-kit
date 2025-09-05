using FSH.Framework.Core.Paging;
using FSH.Starter.WebApi.Warehouse.Domain.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Starter.WebApi.Warehouse.Features.StockMovement.v1;

public static class GetStockMovementListEndpoint
{
    public static void MapGetStockMovementListEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet("/stock-movements", async (
            DefaultIdType? warehouseId,
            string? productSku,
            StockMovementType? movementType,
            StockMovementStatus? status,
            DateTime? fromDate,
            DateTime? toDate,
            int pageNumber = 1,
            int pageSize = 10,
            ISender sender = default!) =>
        {
            var response = await sender.Send(new GetStockMovementListQuery(
                warehouseId, productSku, movementType, status, fromDate, toDate, pageNumber, pageSize));
            return Results.Ok(response);
        })
        .WithName("GetStockMovementList")
        .WithSummary("Get stock movement list")
        .WithDescription("Retrieves a paginated list of stock movements with optional filtering")
        .Produces<PagedList<StockMovementDto>>();
    }
}

public sealed record GetStockMovementListQuery(
    DefaultIdType? WarehouseId,
    string? ProductSku,
    StockMovementType? MovementType,
    StockMovementStatus? Status,
    DateTime? FromDate,
    DateTime? ToDate,
    int PageNumber,
    int PageSize) : IRequest<PagedList<StockMovementDto>>;

public sealed class GetStockMovementListQueryHandler(GetStockMovementListHandler handler)
    : IRequestHandler<GetStockMovementListQuery, PagedList<StockMovementDto>>
{
    public async Task<PagedList<StockMovementDto>> Handle(GetStockMovementListQuery request, CancellationToken cancellationToken)
    {
        return await handler.Handle(new GetStockMovementListRequest(
            request.WarehouseId,
            request.ProductSku,
            request.MovementType,
            request.Status,
            request.FromDate,
            request.ToDate,
            request.PageNumber,
            request.PageSize), cancellationToken);
    }
}
