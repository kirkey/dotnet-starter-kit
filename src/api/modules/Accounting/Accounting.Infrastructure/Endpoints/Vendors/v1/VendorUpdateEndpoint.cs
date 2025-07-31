using Accounting.Application.Vendors.Update.v1;
using FSH.Framework.Infrastructure.Auth.Policy;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Accounting.Infrastructure.Endpoints.Vendors.v1;

public static class VendorUpdateEndpoint
{
    internal static RouteHandlerBuilder MapVendorUpdateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id:guid}", async (DefaultIdType id, VendorUpdateCommand command, ISender mediator) =>
            {
                if (id != command.Id) return Results.BadRequest();
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(VendorUpdateEndpoint))
            .WithSummary("update a vendor")
            .WithDescription("update a vendor")
            .Produces<VendorUpdateResponse>()
            .RequirePermission("Permissions.Accounting.Update")
            .MapToApiVersion(1);
    }
}
