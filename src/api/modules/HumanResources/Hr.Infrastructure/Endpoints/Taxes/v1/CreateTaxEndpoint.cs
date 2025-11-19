namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.Taxes.v1;

using FSH.Starter.WebApi.HumanResources.Application.Taxes.Create.v1;
using Shared.Authorization;

/// <summary>
/// Endpoint for creating a new tax master configuration.
/// </summary>
public static class CreateTaxEndpoint
{
    /// <summary>
    /// Maps the create tax endpoint.
    /// </summary>
    /// <param name="group">Route group builder.</param>
    /// <returns>Route handler builder for further configuration.</returns>
    public static RouteHandlerBuilder MapCreateTaxEndpoint(this RouteGroupBuilder group)
    {
        return group.MapPost("/", async (CreateTaxCommand request, ISender mediator) =>
        {
            var response = await mediator.Send(request).ConfigureAwait(false);
            return Results.CreatedAtRoute(nameof(CreateTaxEndpoint), new { id = response.Id }, response);
        })
        .WithName(nameof(CreateTaxEndpoint))
        .WithSummary("Create tax master configuration")
        .WithDescription("Creates a new tax master configuration for various tax types (VAT, GST, Excise, Withholding, Property, Sales Tax, etc.)")
        .Produces<CreateTaxResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .ProducesProblem(StatusCodes.Status403Forbidden)
        .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Taxes))
        .MapToApiVersion(1);
    }
}

