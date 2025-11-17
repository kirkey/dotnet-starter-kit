using Accounting.Application.Billing.Commands;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.Billing.v1;

public static class CreateInvoiceFromConsumptionEndpoint
{
    internal static RouteHandlerBuilder MapCreateInvoiceFromConsumptionEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/invoices/from-consumption", async (CreateInvoiceFromConsumptionCommand request, ISender mediator) =>
            {
                var id = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(id);
            })
            .WithName(nameof(CreateInvoiceFromConsumptionEndpoint))
            .WithSummary("Create invoice from consumption")
            .WithDescription("Creates an invoice for a consumption record")
            .Produces<DefaultIdType>()
            .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}

