using FSH.Starter.WebApi.HumanResources.Application.BenefitEnrollments.Create.v1;
using FSH.Starter.WebApi.HumanResources.Application.BenefitEnrollments.Get.v1;
using FSH.Starter.WebApi.HumanResources.Application.BenefitEnrollments.Search.v1;
using FSH.Starter.WebApi.HumanResources.Application.BenefitEnrollments.Terminate.v1;
using FSH.Starter.WebApi.HumanResources.Application.BenefitEnrollments.Update.v1;
using Shared.Authorization;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.BenefitEnrollments;

/// <summary>
/// Endpoint routes for managing benefit enrollments.
/// </summary>
public class BenefitEnrollmentsEndpoints() : CarterModule("humanresources")
{
    /// <summary>
    /// Maps all benefit enrollment endpoints to the route builder.
    /// </summary>
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("hr/benefit-enrollments").WithTags("benefit-enrollments");

        group.MapPost("/", async (CreateBenefitEnrollmentCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.CreatedAtRoute("GetBenefitEnrollment", new { id = response.Id }, response);
            })
            .WithName("CreateBenefitEnrollmentEndpoint")
            .WithSummary("Creates a new benefit enrollment")
            .WithDescription("Creates a new benefit enrollment for an employee. Requires approval from HR.")
            .Produces<CreateBenefitEnrollmentResponse>(StatusCodes.Status201Created)
            .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Benefits))
            .MapToApiVersion(1);

        group.MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var request = new GetBenefitEnrollmentRequest(id);
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("GetBenefitEnrollmentEndpoint")
            .WithSummary("Gets a benefit enrollment by ID")
            .WithDescription("Retrieves detailed information about a specific benefit enrollment including status and approval details.")
            .Produces<BenefitEnrollmentResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Benefits))
            .MapToApiVersion(1);

        group.MapPut("/{id:guid}", async (DefaultIdType id, UpdateBenefitEnrollmentCommand request, ISender mediator) =>
            {
                var updateRequest = request with { Id = id };
                var response = await mediator.Send(updateRequest).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("UpdateBenefitEnrollmentEndpoint")
            .WithSummary("Updates a benefit enrollment")
            .WithDescription("Updates a benefit enrollment. Limited updates allowed depending on approval status.")
            .Produces<UpdateBenefitEnrollmentResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Benefits))
            .MapToApiVersion(1);

        group.MapPost("/search", async (SearchBenefitEnrollmentsRequest request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("SearchBenefitEnrollmentsEndpoint")
            .WithSummary("Searches benefit enrollments")
            .WithDescription("Searches and filters benefit enrollments by employee, benefit, status with pagination support.")
            .Produces<PagedList<BenefitEnrollmentResponse>>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Benefits))
            .MapToApiVersion(1);

        group.MapPost("/{id}/terminate", async (DefaultIdType id, ISender mediator) =>
            {
                var request = new TerminateBenefitEnrollmentCommand(id);
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("TerminateBenefitEnrollmentEndpoint")
            .WithSummary("Terminates a benefit enrollment")
            .WithDescription("Terminates an active benefit enrollment, effective immediately or on a specified date.")
            .Produces<TerminateBenefitEnrollmentResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Terminate, FshResources.Benefits))
            .MapToApiVersion(1);
    }
}

