using Carter;
using FSH.Framework.Core.Paging;
using FSH.Starter.WebApi.MicroFinance.Application.MfiConfigurations.Create.v1;
using FSH.Starter.WebApi.MicroFinance.Application.MfiConfigurations.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Application.MfiConfigurations.Search.v1;
using FSH.Starter.WebApi.MicroFinance.Application.MfiConfigurations.Update.v1;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Endpoints;

/// <summary>
/// Endpoint configuration for MFI Configuration.
/// </summary>
public class MfiConfigurationEndpoints : CarterModule
{

    private const string CreateMfiConfiguration = "CreateMfiConfiguration";
    private const string GetMfiConfiguration = "GetMfiConfiguration";
    private const string SearchMfiConfigurations = "SearchMfiConfigurations";
    private const string UpdateMfiConfiguration = "UpdateMfiConfiguration";

    /// <summary>
    /// Maps all MFI Configuration endpoints to the route builder.
    /// </summary>
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("microfinance/configurations").WithTags("MFI Configurations");

        group.MapPost("/", async (CreateMfiConfigurationCommand command, ISender sender) =>
            {
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Created($"/microfinance/configurations/{response.Id}", response);
            })
            .WithName(CreateMfiConfiguration)
            .WithSummary("Creates a new MFI configuration setting")
            .Produces<CreateMfiConfigurationResponse>(StatusCodes.Status201Created)
            .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.MicroFinance))
            .MapToApiVersion(1);

        group.MapGet("/{id:guid}", async (DefaultIdType id, ISender sender) =>
            {
                var response = await sender.Send(new GetMfiConfigurationRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(GetMfiConfiguration)
            .WithSummary("Gets an MFI configuration setting by ID")
            .Produces<MfiConfigurationResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
            .MapToApiVersion(1);

        group.MapPut("/{id:guid}", async (DefaultIdType id, UpdateMfiConfigurationCommand command, ISender sender) =>
            {
                if (id != command.Id) return Results.BadRequest("ID mismatch");
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(UpdateMfiConfiguration)
            .WithSummary("Updates an MFI configuration value")
            .Produces<UpdateMfiConfigurationResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.MicroFinance))
            .MapToApiVersion(1);

        group.MapPost("/search", async (SearchMfiConfigurationsCommand command, ISender sender) =>
            {
                var result = await sender.Send(command).ConfigureAwait(false);
                return Results.Ok(result);
            })
            .WithName(SearchMfiConfigurations)
            .WithSummary("Search MFI configurations")
            .Produces<PagedList<MfiConfigurationSummaryResponse>>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
            .MapToApiVersion(1);
    }
}
