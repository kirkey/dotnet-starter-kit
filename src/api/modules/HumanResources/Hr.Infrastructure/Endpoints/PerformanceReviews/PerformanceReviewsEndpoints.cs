using FSH.Starter.WebApi.HumanResources.Application.PerformanceReviews.Acknowledge.v1;
using FSH.Starter.WebApi.HumanResources.Application.PerformanceReviews.Complete.v1;
using FSH.Starter.WebApi.HumanResources.Application.PerformanceReviews.Create.v1;
using FSH.Starter.WebApi.HumanResources.Application.PerformanceReviews.Get.v1;
using FSH.Starter.WebApi.HumanResources.Application.PerformanceReviews.Search.v1;
using FSH.Starter.WebApi.HumanResources.Application.PerformanceReviews.Submit.v1;
using FSH.Starter.WebApi.HumanResources.Application.PerformanceReviews.Update.v1;
using Shared.Authorization;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.PerformanceReviews;

/// <summary>
/// Endpoint routes for managing performance reviews.
/// </summary>
public class PerformanceReviewsEndpoints() : CarterModule("humanresources")
{
    /// <summary>
    /// Maps all Performance Reviews endpoints to the route builder.
    /// </summary>
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("hr/performance-reviews").WithTags("performance-reviews");

        group.MapPost("/", async (CreatePerformanceReviewCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.CreatedAtRoute("GetPerformanceReview", new { id = response.Id }, response);
            })
            .WithName("CreatePerformanceReviewEndpoint")
            .WithSummary("Creates a new performance review")
            .WithDescription("Creates a new performance review for an employee. Requires manager/reviewer assignment.")
            .Produces<CreatePerformanceReviewResponse>(StatusCodes.Status201Created)
            .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Employees))
            .MapToApiVersion(1);

        group.MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var request = new GetPerformanceReviewRequest(id);
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("GetPerformanceReviewEndpoint")
            .WithSummary("Gets a performance review by ID")
            .WithDescription("Retrieves detailed information about a specific performance review including scores, feedback, and status.")
            .Produces<PerformanceReviewResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Employees))
            .MapToApiVersion(1);

        group.MapPut("/{id:guid}", async (DefaultIdType id, UpdatePerformanceReviewCommand request, ISender mediator) =>
            {
                var updateRequest = request with { Id = id };
                var response = await mediator.Send(updateRequest).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("UpdatePerformanceReviewEndpoint")
            .WithSummary("Updates a performance review")
            .WithDescription("Updates a performance review. Allowed only before submission. Cannot update submitted reviews.")
            .Produces<UpdatePerformanceReviewResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Employees))
            .MapToApiVersion(1);

        group.MapPost("/search", async (SearchPerformanceReviewsRequest request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("SearchPerformanceReviewsEndpoint")
            .WithSummary("Searches performance reviews")
            .WithDescription("Searches and filters performance reviews by employee, period, status, reviewer with pagination support.")
            .Produces<PagedList<PerformanceReviewResponse>>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Employees))
            .MapToApiVersion(1);

        group.MapPost("/{id:guid}/submit", async (DefaultIdType id, ISender mediator) =>
            {
                var request = new SubmitPerformanceReviewCommand(id);
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("SubmitPerformanceReviewEndpoint")
            .WithSummary("Submits a performance review")
            .WithDescription("Submits a draft performance review to the employee. Once submitted, review cannot be edited by reviewer.")
            .Produces<SubmitPerformanceReviewResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Submit, FshResources.Employees))
            .MapToApiVersion(1);

        group.MapPost("/{id:guid}/acknowledge", async (DefaultIdType id, ISender mediator) =>
            {
                var request = new AcknowledgePerformanceReviewCommand(id);
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("AcknowledgePerformanceReviewEndpoint")
            .WithSummary("Acknowledges a performance review")
            .WithDescription("Employee acknowledges receipt of the performance review. Can add comments or disputes.")
            .Produces<AcknowledgePerformanceReviewResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Acknowledge, FshResources.Employees))
            .MapToApiVersion(1);

        group.MapPost("/{id:guid}/complete", async (DefaultIdType id, ISender mediator) =>
            {
                var request = new CompletePerformanceReviewCommand(id);
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("CompletePerformanceReviewEndpoint")
            .WithSummary("Completes a performance review")
            .WithDescription("Marks the performance review as complete. Final step after all approvals and acknowledgments.")
            .Produces<CompletePerformanceReviewResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Complete, FshResources.Employees))
            .MapToApiVersion(1);
    }
}

