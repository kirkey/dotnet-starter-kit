using Carter;
using FSH.Starter.WebApi.MicroFinance.Application.CustomerSurveys.Activate.v1;
using FSH.Starter.WebApi.MicroFinance.Application.CustomerSurveys.Complete.v1;
using FSH.Starter.WebApi.MicroFinance.Application.CustomerSurveys.Create.v1;
using FSH.Starter.WebApi.MicroFinance.Application.CustomerSurveys.Get.v1;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Endpoints;

public class CustomerSurveyEndpoints() : CarterModule
{

    private const string ActivateCustomerSurvey = "ActivateCustomerSurvey";
    private const string CompleteCustomerSurvey = "CompleteCustomerSurvey";
    private const string CreateCustomerSurvey = "CreateCustomerSurvey";
    private const string GetCustomerSurvey = "GetCustomerSurvey";

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("microfinance/customer-surveys").WithTags("Customer Surveys");

        group.MapPost("/", async (CreateCustomerSurveyCommand command, ISender sender) =>
        {
            var result = await sender.Send(command);
            return Results.Created($"/microfinance/customer-surveys/{result.Id}", result);
        })
        .WithName(CreateCustomerSurvey)
        .WithSummary("Create a new customer survey")
        .Produces<CreateCustomerSurveyResponse>(StatusCodes.Status201Created)
        .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapGet("/{id:guid}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new GetCustomerSurveyRequest(id));
            return Results.Ok(result);
        })
        .WithName(GetCustomerSurvey)
        .WithSummary("Get customer survey by ID")
        .Produces<CustomerSurveyResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapPost("/{id:guid}/activate", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new ActivateCustomerSurveyCommand(id));
            return Results.Ok(result);
        })
        .WithName(ActivateCustomerSurvey)
        .WithSummary("Activate customer survey")
        .Produces<ActivateCustomerSurveyResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapPost("/{id:guid}/complete", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new CompleteCustomerSurveyCommand(id));
            return Results.Ok(result);
        })
        .WithName(CompleteCustomerSurvey)
        .WithSummary("Complete customer survey")
        .Produces<CompleteCustomerSurveyResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Complete, FshResources.MicroFinance))
        .MapToApiVersion(1);

    }
}
