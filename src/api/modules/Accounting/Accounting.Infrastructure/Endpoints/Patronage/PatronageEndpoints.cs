using Accounting.Application.Patronages.Commands;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.Patronage;

/// <summary>
/// Endpoint configuration for Patronage module.
/// Provides REST API endpoints for managing patronage distributions and calculations.
/// </summary>
public class PatronageEndpoints : ICarterModule
{
    /// <summary>
    /// Maps all Patronage endpoints to the route builder.
    /// </summary>
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("accounting/patronages").WithTags("patronages");

        // Retire patronage endpoint
        group.MapPost("/retire", async (RetirePatronageCommand request, ISender mediator) =>
            {
                var id = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(id);
            })
            .WithName("RetirePatronage")
            .WithSummary("Retire patronage capital")
            .WithDescription("Process the retirement of patronage capital")
            .Produces<DefaultIdType>()
            .RequirePermission(FshPermission.NameFor(FshActions.Post, FshResources.Accounting));
    }
}
