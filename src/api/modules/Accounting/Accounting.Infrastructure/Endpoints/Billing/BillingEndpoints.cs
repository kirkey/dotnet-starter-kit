using Accounting.Application.Billing.Commands;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.Billing;

/// <summary>
/// Endpoint configuration for Billing module.
/// </summary>
public class BillingEndpoints : ICarterModule
{
    /// <summary>
    /// Maps all Billing endpoints to the route builder.
    /// </summary>
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("accounting/billing").WithTags("billing");

        // Create invoice from consumption endpoint
        group.MapPost("/invoices/from-consumption", async (CreateInvoiceFromConsumptionCommand request, ISender mediator) =>
            {
                var id = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(id);
            })
            .WithName("CreateInvoiceFromConsumption")
            .WithSummary("Create invoice from consumption")
            .WithDescription("Creates an invoice for a consumption record")
            .Produces<DefaultIdType>()
            .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Accounting));
    }
}
