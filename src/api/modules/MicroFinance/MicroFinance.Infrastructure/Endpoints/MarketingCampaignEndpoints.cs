using Carter;
using FSH.Starter.WebApi.MicroFinance.Application.MarketingCampaigns.Approve.v1;
using FSH.Starter.WebApi.MicroFinance.Application.MarketingCampaigns.Complete.v1;
using FSH.Starter.WebApi.MicroFinance.Application.MarketingCampaigns.Create.v1;
using FSH.Starter.WebApi.MicroFinance.Application.MarketingCampaigns.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Application.MarketingCampaigns.Launch.v1;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Endpoints;

public class MarketingCampaignEndpoints() : CarterModule("microfinance")
{

    private const string ApproveMarketingCampaign = "ApproveMarketingCampaign";
    private const string CompleteMarketingCampaign = "CompleteMarketingCampaign";
    private const string CreateMarketingCampaign = "CreateMarketingCampaign";
    private const string GetMarketingCampaign = "GetMarketingCampaign";
    private const string LaunchMarketingCampaign = "LaunchMarketingCampaign";

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("microfinance/marketing-campaigns").WithTags("Marketing Campaigns");

        group.MapPost("/", async (CreateMarketingCampaignCommand command, ISender sender) =>
        {
            var result = await sender.Send(command);
            return Results.Created($"/microfinance/marketing-campaigns/{result.Id}", result);
        })
        .WithName(CreateMarketingCampaign)
        .WithSummary("Create a new marketing campaign")
        .Produces<CreateMarketingCampaignResponse>(StatusCodes.Status201Created);

        group.MapGet("/{id:guid}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new GetMarketingCampaignRequest(id));
            return Results.Ok(result);
        })
        .WithName(GetMarketingCampaign)
        .WithSummary("Get marketing campaign by ID")
        .Produces<MarketingCampaignResponse>();

        group.MapPost("/{id:guid}/approve", async (Guid id, ApproveCampaignRequest request, ISender sender) =>
        {
            var command = new ApproveMarketingCampaignCommand(id, request.ApprovedById);
            var result = await sender.Send(command);
            return Results.Ok(result);
        })
        .WithName(ApproveMarketingCampaign)
        .WithSummary("Approve marketing campaign")
        .Produces<ApproveMarketingCampaignResponse>();

        group.MapPost("/{id:guid}/launch", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new LaunchMarketingCampaignCommand(id));
            return Results.Ok(result);
        })
        .WithName(LaunchMarketingCampaign)
        .WithSummary("Launch marketing campaign")
        .Produces<LaunchMarketingCampaignResponse>();

        group.MapPost("/{id:guid}/complete", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new CompleteMarketingCampaignCommand(id));
            return Results.Ok(result);
        })
        .WithName(CompleteMarketingCampaign)
        .WithSummary("Complete marketing campaign")
        .Produces<CompleteMarketingCampaignResponse>();

    }
}

public sealed record ApproveCampaignRequest(Guid ApprovedById);
