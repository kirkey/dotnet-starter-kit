using FSH.Framework.Core.Identity.Users.Abstractions;
using FSH.Starter.WebApi.HumanResources.Application.BenefitAllocations.Approve.v1;
using FSH.Starter.WebApi.HumanResources.Application.BenefitAllocations.Create.v1;
using FSH.Starter.WebApi.HumanResources.Application.BenefitAllocations.Get.v1;
using FSH.Starter.WebApi.HumanResources.Application.BenefitAllocations.Reject.v1;
using FSH.Starter.WebApi.HumanResources.Application.BenefitAllocations.Search.v1;
using Shared.Authorization;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.BenefitAllocations;

/// <summary>
/// Endpoint routes for managing benefit allocations.
/// </summary>
public class BenefitAllocationsEndpoints() : CarterModule("humanresources")
{
    /// <summary>
    /// Maps all benefit allocation endpoints to the route builder.
    /// </summary>
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("hr/benefit-allocations").WithTags("benefit-allocations");

        group.MapPost("/", async (CreateBenefitAllocationCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.CreatedAtRoute("GetBenefitAllocation", new { id = response.Id }, response);
            })
            .WithName("CreateBenefitAllocationEndpoint")
            .WithSummary("Creates a new benefit allocation")
            .WithDescription("Creates a new benefit allocation for an employee. Allocates specific benefit amounts with optional HR approval.")
            .Produces<CreateBenefitAllocationResponse>(StatusCodes.Status201Created)
            .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Benefits))
            .MapToApiVersion(1);

        group.MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var request = new GetBenefitAllocationRequest(id);
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("GetBenefitAllocationEndpoint")
            .WithSummary("Gets a benefit allocation by ID")
            .WithDescription("Retrieves detailed information about a specific benefit allocation including amount, status, and approval details.")
            .Produces<BenefitAllocationResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Benefits))
            .MapToApiVersion(1);

        group.MapPost("/search", async (SearchBenefitAllocationsRequest request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("SearchBenefitAllocationsEndpoint")
            .WithSummary("Searches benefit allocations")
            .WithDescription("Searches and filters benefit allocations by employee, benefit, status, year with pagination support.")
            .Produces<PagedList<BenefitAllocationResponse>>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Benefits))
            .MapToApiVersion(1);

        group.MapPost("/{id:guid}/approve", async (DefaultIdType id, ICurrentUser currentUser, ISender mediator) =>
            {
                var request = new ApproveBenefitAllocationCommand(id, currentUser.GetUserId());
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("ApproveBenefitAllocationEndpoint")
            .WithSummary("Approves a benefit allocation")
            .WithDescription("Approves a pending benefit allocation. Activates the allocation for the employee.")
            .Produces<ApproveBenefitAllocationResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Approve, FshResources.Benefits))
            .MapToApiVersion(1);

        group.MapPost("/{id}/reject", async (DefaultIdType id, ICurrentUser currentUser, ISender mediator) =>
            {
                var request = new RejectBenefitAllocationCommand(id, currentUser.GetUserId());
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("RejectBenefitAllocationEndpoint")
            .WithSummary("Rejects a benefit allocation")
            .WithDescription("Rejects a pending benefit allocation. Marks as rejected with optional reason.")
            .Produces<RejectBenefitAllocationResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Reject, FshResources.Benefits))
            .MapToApiVersion(1);
    }
}

