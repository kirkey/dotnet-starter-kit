using Carter;
using FSH.Starter.WebApi.HumanResources.Application.PayComponents.Create.v1;
using FSH.Starter.WebApi.HumanResources.Application.PayComponents.Delete.v1;
using FSH.Starter.WebApi.HumanResources.Application.PayComponents.Get.v1;
using FSH.Starter.WebApi.HumanResources.Application.PayComponents.Search.v1;
using FSH.Starter.WebApi.HumanResources.Application.PayComponents.Update.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shared.Authorization;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.PayComponents;

/// <summary>
/// Endpoint configuration for PayComponents module.
/// </summary>
public class PayComponentEndpoints : ICarterModule
{
    /// <summary>
    /// Maps all PayComponents endpoints to the route builder.
    /// </summary>
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("hr/pay-components").WithTags("pay-components");

        group.MapPost("/", async (CreatePayComponentCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Created($"/hr/pay-components/{response.Id}", response);
            })
            .WithName("CreatePayComponent")
            .WithSummary("Create a new pay component")
            .WithDescription("Creates a new pay component for payroll calculation with Philippine labor law compliance")
            .Produces<CreatePayComponentResponse>(StatusCodes.Status201Created)
            .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Payroll))
            .MapToApiVersion(1);

        group.MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetPayComponentRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("GetPayComponent")
            .WithSummary("Get a pay component by ID")
            .WithDescription("Retrieves a specific pay component by its unique identifier")
            .Produces<PayComponentResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Payroll))
            .MapToApiVersion(1);

        group.MapPut("/{id:guid}", async (DefaultIdType id, UpdatePayComponentCommand request, ISender mediator) =>
            {
                var command = request with { Id = id };
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("UpdatePayComponent")
            .WithSummary("Update a pay component")
            .WithDescription("Updates an existing pay component")
            .Produces<UpdatePayComponentResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Payroll))
            .MapToApiVersion(1);

        group.MapDelete("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new DeletePayComponentCommand(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("DeletePayComponent")
            .WithSummary("Delete a pay component")
            .WithDescription("Deletes a pay component by its unique identifier")
            .Produces<DeletePayComponentResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Delete, FshResources.Payroll))
            .MapToApiVersion(1);

        group.MapPost("/search", async (SearchPayComponentsRequest request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("SearchPayComponents")
            .WithSummary("Searches pay components")
            .WithDescription("Searches and filters pay components by type, calculation method, active status with pagination support.")
            .Produces<PagedList<PayComponentResponse>>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Payroll))
            .MapToApiVersion(1);
    }
}

