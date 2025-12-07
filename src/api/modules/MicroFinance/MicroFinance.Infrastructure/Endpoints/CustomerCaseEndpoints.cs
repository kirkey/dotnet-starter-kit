using Carter;
using FSH.Starter.WebApi.MicroFinance.Application.CustomerCases.Assign.v1;
using FSH.Starter.WebApi.MicroFinance.Application.CustomerCases.Close.v1;
using FSH.Starter.WebApi.MicroFinance.Application.CustomerCases.Create.v1;
using FSH.Starter.WebApi.MicroFinance.Application.CustomerCases.Escalate.v1;
using FSH.Starter.WebApi.MicroFinance.Application.CustomerCases.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Application.CustomerCases.Resolve.v1;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Endpoints;

public class CustomerCaseEndpoints() : CarterModule
{

    private const string AssignCustomerCase = "AssignCustomerCase";
    private const string CloseCustomerCase = "CloseCustomerCase";
    private const string CreateCustomerCase = "CreateCustomerCase";
    private const string EscalateCustomerCase = "EscalateCustomerCase";
    private const string GetCustomerCase = "GetCustomerCase";
    private const string ResolveCustomerCase = "ResolveCustomerCase";

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("microfinance/customer-cases").WithTags("Customer Cases");

        group.MapPost("/", async (CreateCustomerCaseCommand command, ISender sender) =>
        {
            var result = await sender.Send(command);
            return Results.Created($"/microfinance/customer-cases/{result.Id}", result);
        })
        .WithName(CreateCustomerCase)
        .WithSummary("Create a new customer case")
        .Produces<CreateCustomerCaseResponse>(StatusCodes.Status201Created)
        .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapGet("/{id:guid}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new GetCustomerCaseRequest(id));
            return Results.Ok(result);
        })
        .WithName(GetCustomerCase)
        .WithSummary("Get customer case by ID")
        .Produces<CustomerCaseResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapPost("/{id:guid}/assign", async (Guid id, AssignCustomerCaseRequest request, ISender sender) =>
        {
            var result = await sender.Send(new AssignCustomerCaseCommand(id, request.StaffId));
            return Results.Ok(result);
        })
        .WithName(AssignCustomerCase)
        .WithSummary("Assign a customer case to staff")
        .Produces<AssignCustomerCaseResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapPost("/{id:guid}/escalate", async (Guid id, EscalateCustomerCaseRequest request, ISender sender) =>
        {
            var result = await sender.Send(new EscalateCustomerCaseCommand(id, request.EscalatedToId, request.Reason));
            return Results.Ok(result);
        })
        .WithName(EscalateCustomerCase)
        .WithSummary("Escalate a customer case")
        .Produces<EscalateCustomerCaseResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapPost("/{id:guid}/resolve", async (Guid id, ResolveCustomerCaseRequest request, ISender sender) =>
        {
            var result = await sender.Send(new ResolveCustomerCaseCommand(id, request.Resolution));
            return Results.Ok(result);
        })
        .WithName(ResolveCustomerCase)
        .WithSummary("Resolve a customer case")
        .Produces<ResolveCustomerCaseResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapPost("/{id:guid}/close", async (Guid id, CloseCustomerCaseRequest request, ISender sender) =>
        {
            var result = await sender.Send(new CloseCustomerCaseCommand(id, request.SatisfactionScore, request.Feedback));
            return Results.Ok(result);
        })
        .WithName(CloseCustomerCase)
        .WithSummary("Close a customer case")
        .Produces<CloseCustomerCaseResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Close, FshResources.MicroFinance))
        .MapToApiVersion(1);

    }
}

public record AssignCustomerCaseRequest(Guid StaffId);
public record EscalateCustomerCaseRequest(Guid EscalatedToId, string Reason);
public record ResolveCustomerCaseRequest(string Resolution);
public record CloseCustomerCaseRequest(int? SatisfactionScore, string? Feedback);
