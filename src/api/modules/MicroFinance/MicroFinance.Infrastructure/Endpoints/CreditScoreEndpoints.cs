using Carter;
using FSH.Framework.Core.Paging;
using FSH.Starter.WebApi.MicroFinance.Application.CreditScores.Create.v1;
using FSH.Starter.WebApi.MicroFinance.Application.CreditScores.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Application.CreditScores.Search.v1;
using FSH.Starter.WebApi.MicroFinance.Application.CreditScores.SetLossParameters.v1;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Endpoints;

public class CreditScoreEndpoints : CarterModule
{

    private const string CreateCreditScore = "CreateCreditScore";
    private const string GetCreditScore = "GetCreditScore";
    private const string SearchCreditScores = "SearchCreditScores";
    private const string SetCreditScoreLossParameters = "SetCreditScoreLossParameters";

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("microfinance/credit-scores").WithTags("Credit Scores");

        group.MapPost("/", async (CreateCreditScoreCommand command, ISender sender) =>
        {
            var result = await sender.Send(command);
            return Results.Created($"/microfinance/credit-scores/{result.Id}", result);
        })
        .WithName(CreateCreditScore)
        .WithSummary("Create a new credit score")
        .Produces<CreateCreditScoreResponse>(StatusCodes.Status201Created)
        .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapGet("/{id:guid}", async (DefaultIdType id, ISender sender) =>
        {
            var result = await sender.Send(new GetCreditScoreRequest(id));
            return Results.Ok(result);
        })
        .WithName(GetCreditScore)
        .WithSummary("Get credit score by ID")
        .Produces<CreditScoreResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapPost("/{id:guid}/loss-parameters", async (DefaultIdType id, SetLossParametersRequest request, ISender sender) =>
        {
            var result = await sender.Send(new SetLossParametersCommand(
                id,
                request.ProbabilityOfDefault,
                request.LossGivenDefault,
                request.ExposureAtDefault));
            return Results.Ok(result);
        })
        .WithName(SetCreditScoreLossParameters)
        .WithSummary("Set loss parameters for risk calculation")
        .Produces<SetLossParametersResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapPost("/search", async (SearchCreditScoresCommand command, ISender sender) =>
        {
            var result = await sender.Send(command).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(SearchCreditScores)
        .WithSummary("Search credit scores")
        .Produces<PagedList<CreditScoreSummaryResponse>>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
        .MapToApiVersion(1);

    }
}

public record SetLossParametersRequest(
    decimal? ProbabilityOfDefault,
    decimal? LossGivenDefault,
    decimal? ExposureAtDefault);
