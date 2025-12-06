using Carter;
using FSH.Framework.Core.Paging;
using FSH.Starter.WebApi.MicroFinance.Application.TellerSessions.Close.v1;
using FSH.Starter.WebApi.MicroFinance.Application.TellerSessions.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Application.TellerSessions.Open.v1;
using FSH.Starter.WebApi.MicroFinance.Application.TellerSessions.Pause.v1;
using FSH.Starter.WebApi.MicroFinance.Application.TellerSessions.RecordCashIn.v1;
using FSH.Starter.WebApi.MicroFinance.Application.TellerSessions.RecordCashOut.v1;
using FSH.Starter.WebApi.MicroFinance.Application.TellerSessions.Resume.v1;
using FSH.Starter.WebApi.MicroFinance.Application.TellerSessions.Search.v1;
using FSH.Starter.WebApi.MicroFinance.Application.TellerSessions.Verify.v1;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Endpoints;

public class TellerSessionEndpoints() : CarterModule("microfinance")
{

    private const string CloseTellerSession = "CloseTellerSession";
    private const string GetTellerSession = "GetTellerSession";
    private const string OpenTellerSession = "OpenTellerSession";
    private const string PauseTellerSession = "PauseTellerSession";
    private const string RecordCashIn = "RecordCashIn";
    private const string RecordCashOut = "RecordCashOut";
    private const string ResumeTellerSession = "ResumeTellerSession";
    private const string SearchTellerSessions = "SearchTellerSessions";
    private const string VerifyTellerSession = "VerifyTellerSession";

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("microfinance/teller-sessions").WithTags("teller-sessions");

        // Session Lifecycle
        group.MapPost("/", async (OpenTellerSessionCommand command, ISender sender) =>
            {
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Created($"/microfinance/teller-sessions/{response.Id}", response);
            })
            .WithName(OpenTellerSession)
            .WithSummary("Opens a new teller session")
            .Produces<OpenTellerSessionResponse>(StatusCodes.Status201Created)
            .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.MicroFinance))
            .MapToApiVersion(1);

        group.MapGet("/{id:guid}", async (Guid id, ISender sender) =>
            {
                var response = await sender.Send(new GetTellerSessionRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(GetTellerSession)
            .WithSummary("Gets a teller session by ID")
            .Produces<TellerSessionResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
            .MapToApiVersion(1);

        // Transaction Recording
        group.MapPost("/{id:guid}/cash-in", async (Guid id, RecordCashInCommand command, ISender sender) =>
            {
                if (id != command.Id) return Results.BadRequest("ID mismatch");
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(RecordCashIn)
            .WithSummary("Records a cash-in transaction")
            .Produces<RecordCashInResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Deposit, FshResources.MicroFinance))
            .MapToApiVersion(1);

        group.MapPost("/{id:guid}/cash-out", async (Guid id, RecordCashOutCommand command, ISender sender) =>
            {
                if (id != command.Id) return Results.BadRequest("ID mismatch");
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(RecordCashOut)
            .WithSummary("Records a cash-out transaction")
            .Produces<RecordCashOutResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Withdraw, FshResources.MicroFinance))
            .MapToApiVersion(1);

        // Session Pause & Resume
        group.MapPost("/{id:guid}/pause", async (Guid id, PauseTellerSessionCommand command, ISender sender) =>
            {
                if (id != command.Id) return Results.BadRequest("ID mismatch");
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(PauseTellerSession)
            .WithSummary("Pauses the teller session temporarily")
            .Produces<PauseTellerSessionResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.MicroFinance))
            .MapToApiVersion(1);

        group.MapPost("/{id:guid}/resume", async (Guid id, ResumeTellerSessionCommand command, ISender sender) =>
            {
                if (id != command.Id) return Results.BadRequest("ID mismatch");
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(ResumeTellerSession)
            .WithSummary("Resumes a paused teller session")
            .Produces<ResumeTellerSessionResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.MicroFinance))
            .MapToApiVersion(1);

        // Session Close & Verification
        group.MapPost("/{id:guid}/close", async (Guid id, CloseTellerSessionCommand command, ISender sender) =>
            {
                if (id != command.Id) return Results.BadRequest("ID mismatch");
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(CloseTellerSession)
            .WithSummary("Closes the teller session with physical count")
            .Produces<CloseTellerSessionResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Close, FshResources.MicroFinance))
            .MapToApiVersion(1);

        group.MapPost("/{id:guid}/verify", async (Guid id, VerifyTellerSessionCommand command, ISender sender) =>
            {
                if (id != command.Id) return Results.BadRequest("ID mismatch");
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(VerifyTellerSession)
            .WithSummary("Supervisor verifies the closed session")
            .Produces<VerifyTellerSessionResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Approve, FshResources.MicroFinance))
            .MapToApiVersion(1);

        // Search
        group.MapPost("/search", async (SearchTellerSessionsCommand command, ISender sender) =>
            {
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(SearchTellerSessions)
            .WithSummary("Searches teller sessions with filtering")
            .Produces<PagedList<TellerSessionResponse>>()
            .RequirePermission(FshPermission.NameFor(FshActions.Search, FshResources.MicroFinance))
            .MapToApiVersion(1);

    }
}
