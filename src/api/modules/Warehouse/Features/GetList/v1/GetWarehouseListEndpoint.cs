using FSH.Framework.Core.Paging;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Starter.WebApi.Warehouse.Features.GetList.v1;

public static class GetWarehouseListEndpoint
{
    public static void MapGetWarehouseListEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet("/", async (
            string? searchTerm,
            bool? isActive,
            int pageNumber = 1,
            int pageSize = 10,
            ISender sender = default!) =>
        {
            var response = await sender.Send(new GetWarehouseListQuery(searchTerm, isActive, pageNumber, pageSize));
            return Results.Ok(response);
        })
        .WithName("GetWarehouseList")
        .WithSummary("Get list of warehouses")
        .WithDescription("Retrieves a paginated list of warehouses with optional filtering")
        .Produces<PagedList<WarehouseDto>>();
    }
}

public sealed record GetWarehouseListQuery(
    string? SearchTerm,
    bool? IsActive,
    int PageNumber,
    int PageSize) : IRequest<PagedList<WarehouseDto>>;

public sealed class GetWarehouseListQueryHandler(GetWarehouseListHandler handler)
    : IRequestHandler<GetWarehouseListQuery, PagedList<WarehouseDto>>
{
    public async Task<PagedList<WarehouseDto>> Handle(GetWarehouseListQuery request, CancellationToken cancellationToken)
    {
        return await handler.Handle(new GetWarehouseListRequest(
            request.SearchTerm,
            request.IsActive,
            request.PageNumber,
            request.PageSize), cancellationToken);
    }
}
