using Carter;
using FSH.Framework.Core.Paging;
using FSH.Starter.WebApi.MicroFinance.Application.CommunicationTemplates.Activate.v1;
using FSH.Starter.WebApi.MicroFinance.Application.CommunicationTemplates.Create.v1;
using FSH.Starter.WebApi.MicroFinance.Application.CommunicationTemplates.Deactivate.v1;
using FSH.Starter.WebApi.MicroFinance.Application.CommunicationTemplates.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Application.CommunicationTemplates.Search.v1;
using FSH.Starter.WebApi.MicroFinance.Application.CommunicationTemplates.Update.v1;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Endpoints;

public class CommunicationTemplateEndpoints : CarterModule
{

    private const string ActivateCommunicationTemplate = "ActivateCommunicationTemplate";
    private const string CreateCommunicationTemplate = "CreateCommunicationTemplate";
    private const string DeactivateCommunicationTemplate = "DeactivateCommunicationTemplate";
    private const string GetCommunicationTemplate = "GetCommunicationTemplate";
    private const string SearchCommunicationTemplates = "SearchCommunicationTemplates";
    private const string UpdateCommunicationTemplate = "UpdateCommunicationTemplate";

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

        group.MapGet("/{id:guid}", async (DefaultIdType id, ISender sender) =>
        {
            var result = await sender.Send(new GetCommunicationTemplateRequest(id));
            return Results.Ok(result);
        })
        .WithName(GetCommunicationTemplate)
        .WithSummary("Get communication template by ID")
        .Produces<CommunicationTemplateResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapPost("/{id:guid}/activate", async (DefaultIdType id, ISender sender) =>
        {
            var result = await sender.Send(new ActivateTemplateCommand(id));
            return Results.Ok(result);
        })
        .WithName(ActivateCommunicationTemplate)
        .WithSummary("Activate a communication template")
        .Produces<ActivateTemplateResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapPost("/search", async (SearchCommunicationTemplatesCommand command, ISender sender) =>
        {
            var result = await sender.Send(command).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(SearchCommunicationTemplates)
        .WithSummary("Search communication templates")
        .Produces<PagedList<CommunicationTemplateSummaryResponse>>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapPut("/{id:guid}", async (DefaultIdType id, UpdateCommunicationTemplateRequest request, ISender sender) =>
        {
            var result = await sender.Send(new UpdateCommunicationTemplateCommand(
                id,
                request.Name,
                request.Subject,
                request.Body,
                request.Placeholders,
                request.RequiresApproval,
                request.Notes)).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(UpdateCommunicationTemplate)
        .WithSummary("Update a communication template")
        .Produces<UpdateCommunicationTemplateResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapPost("/{id:guid}/deactivate", async (DefaultIdType id, ISender sender) =>
        {
            var result = await sender.Send(new DeactivateCommunicationTemplateCommand(id)).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(DeactivateCommunicationTemplate)
        .WithSummary("Deactivate a communication template")
        .Produces<DeactivateCommunicationTemplateResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.MicroFinance))
        .MapToApiVersion(1);

    }
}

public sealed record UpdateCommunicationTemplateRequest(
    string? Name,
    string? Subject,
    string? Body,
    string? Placeholders,
    bool? RequiresApproval,
    string? Notes);
