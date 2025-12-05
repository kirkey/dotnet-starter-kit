using Carter;
using FSH.Starter.WebApi.MicroFinance.Application.MfiConfigurations.Create.v1;
using FSH.Starter.WebApi.MicroFinance.Application.MfiConfigurations.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Application.MfiConfigurations.Update.v1;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Endpoints;

/// <summary>
/// Endpoint configuration for MFI Configuration.
/// </summary>
public class MfiConfigurationEndpoints() : CarterModule("microfinance")
{
    /// <summary>
    /// Maps all MFI Configuration endpoints to the route builder.
    /// </summary>
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("microfinance/configurations").WithTags("mfi-configurations");

        group.MapPost("/", async (CreateMfiConfigurationCommand command, ISender sender) =>
            {
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Created($"/microfinance/configurations/{response.Id}", response);
            })
            .WithName("CreateMfiConfiguration")
            .WithSummary("Creates a new MFI configuration setting")
            .Produces<CreateMfiConfigurationResponse>(StatusCodes.Status201Created);

        group.MapGet("/{id:guid}", async (Guid id, ISender sender) =>
            {
                var response = await sender.Send(new GetMfiConfigurationRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("GetMfiConfiguration")
            .WithSummary("Gets an MFI configuration setting by ID")
            .Produces<MfiConfigurationResponse>();

        group.MapPut("/{id:guid}", async (Guid id, UpdateMfiConfigurationCommand command, ISender sender) =>
            {
                if (id != command.Id) return Results.BadRequest("ID mismatch");
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("UpdateMfiConfiguration")
            .WithSummary("Updates an MFI configuration value")
            .Produces<UpdateMfiConfigurationResponse>();
    }
}
