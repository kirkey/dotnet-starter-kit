using Accounting.Application.Vendors.Get.v1;
using Accounting.Application.Vendors.Search.v1;
using FSH.Framework.Core.Paging;
using FSH.Framework.Infrastructure.Auth.Policy;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Accounting.Infrastructure.Endpoints.Vendors.v1;

public static class VendorSearchEndpoint
{
    internal static RouteHandlerBuilder MapVendorSearchEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (ISender mediator, [FromBody] VendorSearchQuery command) =>
            {
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(VendorSearchEndpoint))
            .WithSummary("Gets a list of vendors")
            .WithDescription("Gets a list of vendors with pagination and filtering support")
            .Produces<PagedList<VendorSearchResponse>>()
            .RequirePermission("Permissions.Accounting.View")
            .MapToApiVersion(1);
    }
}
