using Carter;
using FSH.Starter.WebApi.MicroFinance.Application.AgentBankings.Approve.v1;
using FSH.Starter.WebApi.MicroFinance.Application.AgentBankings.Create.v1;
using FSH.Starter.WebApi.MicroFinance.Application.AgentBankings.CreditFloat.v1;
using FSH.Starter.WebApi.MicroFinance.Application.AgentBankings.DebitFloat.v1;
using FSH.Starter.WebApi.MicroFinance.Application.AgentBankings.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Application.AgentBankings.RecordAudit.v1;
using FSH.Starter.WebApi.MicroFinance.Application.AgentBankings.Search.v1;
using FSH.Starter.WebApi.MicroFinance.Application.AgentBankings.Suspend.v1;
using FSH.Starter.WebApi.MicroFinance.Application.AgentBankings.Update.v1;
using FSH.Starter.WebApi.MicroFinance.Application.AgentBankings.UpgradeTier.v1;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Endpoints;

/// <summary>
/// Endpoint configuration for Agent Banking.
/// </summary>
public class AgentBankingEndpoints() : CarterModule
{

    private const string ApproveAgentBanking = "ApproveAgentBanking";
    private const string CreateAgentBanking = "CreateAgentBanking";
    private const string CreditAgentFloat = "CreditAgentFloat";
    private const string DebitAgentFloat = "DebitAgentFloat";
    private const string GetAgentBanking = "GetAgentBanking";
    private const string RecordAuditAgentBanking = "RecordAuditAgentBanking";
    private const string SearchAgentBankings = "SearchAgentBankings";
    private const string SuspendAgentBanking = "SuspendAgentBanking";
    private const string UpdateAgentBanking = "UpdateAgentBanking";
    private const string UpgradeTierAgentBanking = "UpgradeTierAgentBanking";

    /// <summary>
    /// Maps all Agent Banking endpoints to the route builder.
    /// </summary>
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("microfinance/agent-banking").WithTags("agent-banking");

        group.MapPost("/", async (CreateAgentBankingCommand command, ISender sender) =>
            {
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Created($"/microfinance/agent-banking/{response.Id}", response);
            })
            .WithName(CreateAgentBanking)
            .WithSummary("Creates a new agent banking location")
            .Produces<CreateAgentBankingResponse>(StatusCodes.Status201Created)
            .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.MicroFinance))
            .MapToApiVersion(1);

        group.MapGet("/{id:guid}", async (Guid id, ISender sender) =>
            {
                var response = await sender.Send(new GetAgentBankingRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(GetAgentBanking)
            .WithSummary("Gets an agent banking location by ID")
            .Produces<AgentBankingResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
            .MapToApiVersion(1);

        group.MapPost("/{id:guid}/approve", async (Guid id, ISender sender) =>
            {
                var response = await sender.Send(new ApproveAgentBankingCommand(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(ApproveAgentBanking)
            .WithSummary("Approves an agent banking location")
            .Produces<ApproveAgentBankingResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Approve, FshResources.MicroFinance))
            .MapToApiVersion(1);

        group.MapPost("/{id:guid}/suspend", async (Guid id, SuspendAgentRequest request, ISender sender) =>
            {
                var response = await sender.Send(new SuspendAgentBankingCommand(id, request.Reason)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(SuspendAgentBanking)
            .WithSummary("Suspends an agent banking location")
            .Produces<SuspendAgentBankingResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
            .MapToApiVersion(1);

        group.MapPost("/{id:guid}/credit-float", async (Guid id, FloatAmountRequest request, ISender sender) =>
            {
                var response = await sender.Send(new CreditFloatCommand(id, request.Amount)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(CreditAgentFloat)
            .WithSummary("Credits float to an agent's account")
            .Produces<CreditFloatResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
            .MapToApiVersion(1);

        group.MapPost("/{id:guid}/debit-float", async (Guid id, FloatAmountRequest request, ISender sender) =>
            {
                var response = await sender.Send(new DebitFloatCommand(id, request.Amount)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(DebitAgentFloat)
            .WithSummary("Debits float from an agent's account")
            .Produces<DebitFloatResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Withdraw, FshResources.MicroFinance))
            .MapToApiVersion(1);

        group.MapPut("/{id:guid}", async (Guid id, UpdateAgentBankingCommand command, ISender sender) =>
            {
                if (id != command.Id) return Results.BadRequest("ID mismatch");
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(UpdateAgentBanking)
            .WithSummary("Updates an agent banking location")
            .Produces<UpdateAgentBankingResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.MicroFinance))
            .MapToApiVersion(1);

        group.MapPost("/{id:guid}/upgrade-tier", async (Guid id, UpgradeTierAgentRequest request, ISender sender) =>
            {
                var response = await sender.Send(new UpgradeTierAgentBankingCommand(id, request.NewTier)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(UpgradeTierAgentBanking)
            .WithSummary("Upgrades an agent's tier")
            .Produces<UpgradeTierAgentBankingResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.MicroFinance))
            .MapToApiVersion(1);

        group.MapPost("/{id:guid}/record-audit", async (Guid id, RecordAuditAgentRequest request, ISender sender) =>
            {
                var response = await sender.Send(new RecordAuditAgentBankingCommand(id, request.AuditDate)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(RecordAuditAgentBanking)
            .WithSummary("Records an audit for an agent")
            .Produces<RecordAuditAgentBankingResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.MicroFinance))
            .MapToApiVersion(1);

        group.MapPost("/search", async (SearchAgentBankingsCommand command, ISender sender) =>
            {
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(SearchAgentBankings)
            .WithSummary("Searches agent banking locations with filters and pagination")
            .Produces<PagedList<AgentBankingResponse>>()
            .RequirePermission(FshPermission.NameFor(FshActions.Search, FshResources.MicroFinance))
            .MapToApiVersion(1);
    }
}

public sealed record SuspendAgentRequest(string Reason);
public sealed record FloatAmountRequest(decimal Amount);
public sealed record UpgradeTierAgentRequest(string NewTier);
public sealed record RecordAuditAgentRequest(DateOnly AuditDate);
