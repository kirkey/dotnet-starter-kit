using Carter;
using FSH.Starter.WebApi.MicroFinance.Application.UssdSessions.Create.v1;
using FSH.Starter.WebApi.MicroFinance.Application.UssdSessions.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Application.UssdSessions.Search.v1;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Endpoints;

/// <summary>
/// Endpoint configuration for USSD Sessions.
/// </summary>
public class UssdSessionEndpoints : CarterModule
{
    private const string CreateUssdSession = "CreateUssdSession";
    private const string GetUssdSession = "GetUssdSession";
    private const string SearchUssdSessions = "SearchUssdSessions";

    /// <summary>
    /// Maps all USSD Session endpoints to the route builder.
    /// </summary>
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("microfinance/ussd-sessions").WithTags("USSD Sessions");

        group.MapPost("/", async (CreateUssdSessionCommand command, ISender sender) =>
            {
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Created($"/microfinance/ussd-sessions/{response.Id}", response);
            })
            .WithName(CreateUssdSession)
            .WithSummary("Creates a new USSD session")
            .Produces<CreateUssdSessionResponse>(StatusCodes.Status201Created)
            .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.MicroFinance))
            .MapToApiVersion(1);

        group.MapGet("/{id:guid}", async (DefaultIdType id, ISender sender) =>
            {
                var response = await sender.Send(new GetUssdSessionRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(GetUssdSession)
            .WithSummary("Gets a USSD session by ID")
            .Produces<UssdSessionResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
            .MapToApiVersion(1);

        group.MapPost("/search", async ([FromBody] SearchUssdSessionsCommand command, ISender sender) =>
            {
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(SearchUssdSessions)
            .WithSummary("Searches USSD sessions with filters and pagination")
            .Produces<PagedList<UssdSessionResponse>>()
            .RequirePermission(FshPermission.NameFor(FshActions.Search, FshResources.MicroFinance))
            .MapToApiVersion(1);
    }
}
