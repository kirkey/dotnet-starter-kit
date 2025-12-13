using Carter;
using FSH.Framework.Core.Paging;
using FSH.Starter.WebApi.MicroFinance.Application.CustomerSegments.Activate.v1;
using FSH.Starter.WebApi.MicroFinance.Application.CustomerSegments.Create.v1;
using FSH.Starter.WebApi.MicroFinance.Application.CustomerSegments.Deactivate.v1;
using FSH.Starter.WebApi.MicroFinance.Application.CustomerSegments.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Application.CustomerSegments.Search.v1;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Endpoints;

public class CustomerSegmentEndpoints : CarterModule
{

    private const string ActivateCustomerSegment = "ActivateCustomerSegment";
    private const string CreateCustomerSegment = "CreateCustomerSegment";
    private const string DeactivateCustomerSegment = "DeactivateCustomerSegment";
    private const string GetCustomerSegment = "GetCustomerSegment";
    private const string SearchCustomerSegments = "SearchCustomerSegments";

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("microfinance/customer-segments").WithTags("Customer Segments");

        group.MapPost("/", async (CreateCustomerSegmentCommand command, ISender sender) =>
        {
            var result = await sender.Send(command);
            return Results.Created($"/microfinance/customer-segments/{result.Id}", result);
        })
        .WithName(CreateCustomerSegment)
        .WithSummary("Create a new customer segment")
        .Produces<CreateCustomerSegmentResponse>(StatusCodes.Status201Created)
        .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapGet("/{id:guid}", async (DefaultIdType id, ISender sender) =>
        {
            var result = await sender.Send(new GetCustomerSegmentRequest(id));
            return Results.Ok(result);
        })
        .WithName(GetCustomerSegment)
        .WithSummary("Get customer segment by ID")
        .Produces<CustomerSegmentResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapPost("/{id}/activate", async (DefaultIdType id, ISender sender) =>
        {
            var result = await sender.Send(new ActivateCustomerSegmentCommand(id));
            return Results.Ok(result);
        })
        .WithName(ActivateCustomerSegment)
        .WithSummary("Activate customer segment")
        .Produces<ActivateCustomerSegmentResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapPost("/{id}/deactivate", async (DefaultIdType id, ISender sender) =>
        {
            var result = await sender.Send(new DeactivateCustomerSegmentCommand(id));
            return Results.Ok(result);
        })
        .WithName(DeactivateCustomerSegment)
        .WithSummary("Deactivate customer segment")
        .Produces<DeactivateCustomerSegmentResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapPost("/search", async (SearchCustomerSegmentsCommand command, ISender sender) =>
        {
            var result = await sender.Send(command).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(SearchCustomerSegments)
        .WithSummary("Search customer segments")
        .Produces<PagedList<CustomerSegmentSummaryResponse>>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
        .MapToApiVersion(1);

    }
}
