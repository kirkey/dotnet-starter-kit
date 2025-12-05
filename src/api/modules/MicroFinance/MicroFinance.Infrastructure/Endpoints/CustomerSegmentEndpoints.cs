using Carter;
using FSH.Starter.WebApi.MicroFinance.Application.CustomerSegments.Activate.v1;
using FSH.Starter.WebApi.MicroFinance.Application.CustomerSegments.Create.v1;
using FSH.Starter.WebApi.MicroFinance.Application.CustomerSegments.Deactivate.v1;
using FSH.Starter.WebApi.MicroFinance.Application.CustomerSegments.Get.v1;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Endpoints;

public class CustomerSegmentEndpoints() : CarterModule("microfinance")
{

    private const string ActivateCustomerSegment = "ActivateCustomerSegment";
    private const string CreateCustomerSegment = "CreateCustomerSegment";
    private const string DeactivateCustomerSegment = "DeactivateCustomerSegment";
    private const string GetCustomerSegment = "GetCustomerSegment";

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
        .Produces<CreateCustomerSegmentResponse>(StatusCodes.Status201Created);

        group.MapGet("/{id:guid}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new GetCustomerSegmentRequest(id));
            return Results.Ok(result);
        })
        .WithName(GetCustomerSegment)
        .WithSummary("Get customer segment by ID")
        .Produces<CustomerSegmentResponse>();

        group.MapPost("/{id:guid}/activate", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new ActivateCustomerSegmentCommand(id));
            return Results.Ok(result);
        })
        .WithName(ActivateCustomerSegment)
        .WithSummary("Activate customer segment")
        .Produces<ActivateCustomerSegmentResponse>();

        group.MapPost("/{id:guid}/deactivate", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new DeactivateCustomerSegmentCommand(id));
            return Results.Ok(result);
        })
        .WithName(DeactivateCustomerSegment)
        .WithSummary("Deactivate customer segment")
        .Produces<DeactivateCustomerSegmentResponse>();

    }
}
