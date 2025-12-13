using Carter;
using FSH.Starter.WebApi.MicroFinance.Application.CollectionCases.Assign.v1;
using FSH.Starter.WebApi.MicroFinance.Application.CollectionCases.Close.v1;
using FSH.Starter.WebApi.MicroFinance.Application.CollectionCases.Create.v1;
using FSH.Starter.WebApi.MicroFinance.Application.CollectionCases.EscalateToLegal.v1;
using FSH.Starter.WebApi.MicroFinance.Application.CollectionCases.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Application.CollectionCases.RecordContact.v1;
using FSH.Starter.WebApi.MicroFinance.Application.CollectionCases.RecordRecovery.v1;
using FSH.Starter.WebApi.MicroFinance.Application.CollectionCases.Search.v1;
using FSH.Starter.WebApi.MicroFinance.Application.CollectionCases.Settle.v1;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Endpoints;

public class CollectionCaseEndpoints : CarterModule
{

    private const string AssignCollectionCase = "AssignCollectionCase";
    private const string CloseCollectionCase = "CloseCollectionCase";
    private const string CreateCollectionCase = "CreateCollectionCase";
    private const string EscalateToLegal = "EscalateToLegal";
    private const string GetCollectionCase = "GetCollectionCase";
    private const string RecordContact = "RecordContact";
    private const string RecordCollectionCaseRecovery = "RecordCollectionCaseRecovery";
    private const string SearchCollectionCases = "SearchCollectionCases";
    private const string SettleCollectionCase = "SettleCollectionCase";

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("microfinance/collection-cases").WithTags("Collection Cases");

        // Case Management
        group.MapPost("/", async (CreateCollectionCaseCommand command, ISender sender) =>
            {
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Created($"/microfinance/collection-cases/{response.Id}", response);
            })
            .WithName(CreateCollectionCase)
            .WithSummary("Creates a new collection case for a delinquent loan")
            .Produces<CreateCollectionCaseResponse>(StatusCodes.Status201Created)
            .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.MicroFinance))
            .MapToApiVersion(1);

        group.MapGet("/{id:guid}", async (DefaultIdType id, ISender sender) =>
            {
                var response = await sender.Send(new GetCollectionCaseRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(GetCollectionCase)
            .WithSummary("Gets a collection case by ID")
            .Produces<CollectionCaseResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
            .MapToApiVersion(1);

        // Assignment
        group.MapPost("/{id}/assign", async (DefaultIdType id, AssignCollectionCaseCommand command, ISender sender) =>
            {
                if (id != command.Id) return Results.BadRequest("ID mismatch");
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(AssignCollectionCase)
            .WithSummary("Assigns the case to a collector")
            .Produces<AssignCollectionCaseResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.MicroFinance))
            .MapToApiVersion(1);

        // Collection Activities
        group.MapPost("/{id}/record-contact", async (DefaultIdType id, RecordContactCommand command, ISender sender) =>
            {
                if (id != command.Id) return Results.BadRequest("ID mismatch");
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(RecordContact)
            .WithSummary("Records a contact attempt with the borrower")
            .Produces<RecordContactResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.MicroFinance))
            .MapToApiVersion(1);

        group.MapPost("/{id}/record-recovery", async (DefaultIdType id, RecordCollectionCaseRecoveryCommand command, ISender sender) =>
            {
                if (id != command.Id) return Results.BadRequest("ID mismatch");
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(RecordCollectionCaseRecovery)
            .WithSummary("Records a payment recovery")
            .Produces<RecordCollectionCaseRecoveryResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.MicroFinance))
            .MapToApiVersion(1);

        // Escalation & Resolution
        group.MapPost("/{id}/escalate-legal", async (DefaultIdType id, EscalateToLegalCommand command, ISender sender) =>
            {
                if (id != command.Id) return Results.BadRequest("ID mismatch");
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(EscalateToLegal)
            .WithSummary("Escalates the case to legal action")
            .Produces<EscalateToLegalResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Process, FshResources.MicroFinance))
            .MapToApiVersion(1);

        group.MapPost("/{id}/settle", async (DefaultIdType id, SettleCollectionCaseCommand command, ISender sender) =>
            {
                if (id != command.Id) return Results.BadRequest("ID mismatch");
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(SettleCollectionCase)
            .WithSummary("Settles the case with agreed terms")
            .Produces<SettleCollectionCaseResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Process, FshResources.MicroFinance))
            .MapToApiVersion(1);

        group.MapPost("/{id}/close", async (DefaultIdType id, CloseCollectionCaseCommand command, ISender sender) =>
            {
                if (id != command.Id) return Results.BadRequest("ID mismatch");
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(CloseCollectionCase)
            .WithSummary("Closes the collection case")
            .Produces<CloseCollectionCaseResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Close, FshResources.MicroFinance))
            .MapToApiVersion(1);

        // Search
        group.MapPost("/search", async (SearchCollectionCasesCommand command, ISender sender) =>
            {
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(SearchCollectionCases)
            .WithSummary("Searches collection cases with filtering")
            .Produces<PagedList<CollectionCaseResponse>>()
            .RequirePermission(FshPermission.NameFor(FshActions.Search, FshResources.MicroFinance))
            .MapToApiVersion(1);

    }
}
