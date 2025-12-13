using Carter;
using FSH.Starter.WebApi.MicroFinance.Application.LegalActions.Create.v1;
using FSH.Starter.WebApi.MicroFinance.Application.LegalActions.FileCase.v1;
using FSH.Starter.WebApi.MicroFinance.Application.LegalActions.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Application.LegalActions.Search.v1;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Endpoints;

/// <summary>
/// Endpoint configuration for Legal Actions.
/// </summary>
public class LegalActionEndpoints : CarterModule
{
    private const string CreateLegalAction = "CreateLegalAction";
    private const string FileCase = "FileCase";
    private const string GetLegalAction = "GetLegalAction";
    private const string SearchLegalActions = "SearchLegalActions";

    /// <summary>
    /// Maps all Legal Action endpoints to the route builder.
    /// </summary>
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("microfinance/legal-actions").WithTags("Legal Actions");

        group.MapPost("/", async (CreateLegalActionCommand command, ISender sender) =>
        {
            var result = await sender.Send(command).ConfigureAwait(false);
            return Results.Created($"/microfinance/legal-actions/{result.Id}", result);
        })
        .WithName(CreateLegalAction)
        .WithSummary("Create a new legal action")
        .Produces<CreateLegalActionResponse>(StatusCodes.Status201Created)
        .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapGet("/{id:guid}", async (DefaultIdType id, ISender sender) =>
        {
            var result = await sender.Send(new GetLegalActionRequest(id)).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(GetLegalAction)
        .WithSummary("Get legal action by ID")
        .Produces<LegalActionResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapPost("/search", async ([FromBody] SearchLegalActionsCommand command, ISender sender) =>
        {
            var result = await sender.Send(command).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(SearchLegalActions)
        .WithSummary("Search legal actions with filters and pagination")
        .Produces<PagedList<LegalActionResponse>>()
        .RequirePermission(FshPermission.NameFor(FshActions.Search, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapPost("/{id}/file-case", async (DefaultIdType id, FileCaseRequest request, ISender sender) =>
        {
            var command = new FileCaseCommand(id, request.FiledDate, request.CaseReference, request.CourtName, request.CourtFees);
            var result = await sender.Send(command).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(FileCase)
        .WithSummary("File legal case with court")
        .Produces<FileCaseResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.MicroFinance))
        .MapToApiVersion(1);
    }
}

public sealed record FileCaseRequest(DateOnly FiledDate, string CaseReference, string CourtName, decimal CourtFees);
