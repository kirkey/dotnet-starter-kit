using Carter;
using FSH.Framework.Core.Paging;
using FSH.Starter.WebApi.MicroFinance.Application.FeeDefinitions.Create.v1;
using FSH.Starter.WebApi.MicroFinance.Application.FeeDefinitions.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Application.FeeDefinitions.Search.v1;
using FSH.Starter.WebApi.MicroFinance.Application.FeeDefinitions.Update.v1;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Endpoints;

/// <summary>
/// Endpoint configuration for Fee Definitions.
/// </summary>
public class FeeDefinitionEndpoints : ICarterModule
{
    /// <summary>
    /// Maps all Fee Definition endpoints to the route builder.
    /// </summary>
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var feeDefinitionsGroup = app.MapGroup("microfinance/fee-definitions").WithTags("fee-definitions");

        feeDefinitionsGroup.MapPost("/", async (CreateFeeDefinitionCommand request, ISender mediator) =>
        {
            var response = await mediator.Send(request);
            return Results.Created($"/api/microfinance/fee-definitions/{response}", new { id = response });
        })
        .WithName("CreateFeeDefinition")
        .WithSummary("Creates a new fee definition")
        .Produces<Guid>(StatusCodes.Status201Created);

        feeDefinitionsGroup.MapGet("/{id:guid}", async (Guid id, ISender mediator) =>
        {
            var response = await mediator.Send(new GetFeeDefinitionRequest(id));
            return Results.Ok(response);
        })
        .WithName("GetFeeDefinition")
        .WithSummary("Gets a fee definition by ID")
        .Produces<FeeDefinitionResponse>();

        feeDefinitionsGroup.MapPost("/search", async (SearchFeeDefinitionsCommand request, ISender mediator) =>
        {
            var response = await mediator.Send(request);
            return Results.Ok(response);
        })
        .WithName("SearchFeeDefinitions")
        .WithSummary("Searches fee definitions with pagination")
        .Produces<PagedList<FeeDefinitionResponse>>();

        feeDefinitionsGroup.MapPut("/{id:guid}", async (Guid id, UpdateFeeDefinitionCommand request, ISender mediator) =>
        {
            if (id != request.Id)
            {
                return Results.BadRequest("ID mismatch");
            }
            await mediator.Send(request);
            return Results.NoContent();
        })
        .WithName("UpdateFeeDefinition")
        .WithSummary("Updates a fee definition")
        .Produces(StatusCodes.Status204NoContent);

    }
}
