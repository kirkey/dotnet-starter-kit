using Carter;
using FSH.Starter.WebApi.MicroFinance.Application.CommunicationTemplates.Activate.v1;
using FSH.Starter.WebApi.MicroFinance.Application.CommunicationTemplates.Create.v1;
using FSH.Starter.WebApi.MicroFinance.Application.CommunicationTemplates.Get.v1;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Endpoints;

public class CommunicationTemplateEndpoints() : CarterModule("microfinance")
{

    private const string ActivateCommunicationTemplate = "ActivateCommunicationTemplate";
    private const string CreateCommunicationTemplate = "CreateCommunicationTemplate";
    private const string GetCommunicationTemplate = "GetCommunicationTemplate";

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("microfinance/communication-templates").WithTags("Communication Templates");

        group.MapPost("/", async (CreateCommunicationTemplateCommand command, ISender sender) =>
        {
            var result = await sender.Send(command);
            return Results.Created($"/microfinance/communication-templates/{result.Id}", result);
        })
        .WithName(CreateCommunicationTemplate)
        .WithSummary("Create a new communication template")
        .Produces<CreateCommunicationTemplateResponse>(StatusCodes.Status201Created)
        .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapGet("/{id:guid}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new GetCommunicationTemplateRequest(id));
            return Results.Ok(result);
        })
        .WithName(GetCommunicationTemplate)
        .WithSummary("Get communication template by ID")
        .Produces<CommunicationTemplateResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapPost("/{id:guid}/activate", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new ActivateTemplateCommand(id));
            return Results.Ok(result);
        })
        .WithName(ActivateCommunicationTemplate)
        .WithSummary("Activate a communication template")
        .Produces<ActivateTemplateResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
        .MapToApiVersion(1);

    }
}
