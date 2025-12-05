using Carter;
using FSH.Starter.WebApi.MicroFinance.Application.CollectionCases.Assign.v1;
using FSH.Starter.WebApi.MicroFinance.Application.CollectionCases.Create.v1;
using FSH.Starter.WebApi.MicroFinance.Application.CollectionCases.EscalateToLegal.v1;
using FSH.Starter.WebApi.MicroFinance.Application.CollectionCases.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Application.CollectionCases.RecordContact.v1;
using FSH.Starter.WebApi.MicroFinance.Application.CollectionCases.RecordRecovery.v1;
using FSH.Starter.WebApi.MicroFinance.Application.CollectionCases.Settle.v1;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Endpoints;

public class CollectionCaseEndpoints() : CarterModule("microfinance")
{

    private const string AssignCollectionCase = "AssignCollectionCase";
    private const string CreateCollectionCase = "CreateCollectionCase";
    private const string EscalateToLegal = "EscalateToLegal";
    private const string GetCollectionCase = "GetCollectionCase";
    private const string RecordContact = "RecordContact";
    private const string RecordRecovery = "RecordRecovery";
    private const string SettleCollectionCase = "SettleCollectionCase";

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("microfinance/collection-cases").WithTags("collection-cases");

        // Case Management
        group.MapPost("/", async (CreateCollectionCaseCommand command, ISender sender) =>
            {
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Created($"/microfinance/collection-cases/{response.Id}", response);
            })
            .WithName(CreateCollectionCase)
            .WithSummary("Creates a new collection case for a delinquent loan")
            .Produces<CreateCollectionCaseResponse>(StatusCodes.Status201Created);

        group.MapGet("/{id:guid}", async (Guid id, ISender sender) =>
            {
                var response = await sender.Send(new GetCollectionCaseRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(GetCollectionCase)
            .WithSummary("Gets a collection case by ID")
            .Produces<CollectionCaseResponse>();

        // Assignment
        group.MapPost("/{id:guid}/assign", async (Guid id, AssignCollectionCaseCommand command, ISender sender) =>
            {
                if (id != command.Id) return Results.BadRequest("ID mismatch");
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(AssignCollectionCase)
            .WithSummary("Assigns the case to a collector")
            .Produces<AssignCollectionCaseResponse>();

        // Collection Activities
        group.MapPost("/{id:guid}/record-contact", async (Guid id, RecordContactCommand command, ISender sender) =>
            {
                if (id != command.Id) return Results.BadRequest("ID mismatch");
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(RecordContact)
            .WithSummary("Records a contact attempt with the borrower")
            .Produces<RecordContactResponse>();

        group.MapPost("/{id:guid}/record-recovery", async (Guid id, RecordRecoveryCommand command, ISender sender) =>
            {
                if (id != command.Id) return Results.BadRequest("ID mismatch");
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(RecordRecovery)
            .WithSummary("Records a payment recovery")
            .Produces<RecordRecoveryResponse>();

        // Escalation & Resolution
        group.MapPost("/{id:guid}/escalate-legal", async (Guid id, EscalateToLegalCommand command, ISender sender) =>
            {
                if (id != command.Id) return Results.BadRequest("ID mismatch");
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(EscalateToLegal)
            .WithSummary("Escalates the case to legal action")
            .Produces<EscalateToLegalResponse>();

        group.MapPost("/{id:guid}/settle", async (Guid id, SettleCollectionCaseCommand command, ISender sender) =>
            {
                if (id != command.Id) return Results.BadRequest("ID mismatch");
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(SettleCollectionCase)
            .WithSummary("Settles the case with agreed terms")
            .Produces<SettleCollectionCaseResponse>();

    }
}
