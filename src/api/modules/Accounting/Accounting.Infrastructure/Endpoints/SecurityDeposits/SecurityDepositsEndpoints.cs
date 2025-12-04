using Accounting.Application.SecurityDeposits.Commands;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.SecurityDeposits;

/// <summary>
/// Endpoint configuration for Security Deposits module.
/// Provides comprehensive REST API endpoints for managing customer security deposits.
/// </summary>
public class SecurityDepositsEndpoints : ICarterModule
{
    /// <summary>
    /// Maps all Security Deposits endpoints to the route builder.
    /// Includes Create operation for security deposits.
    /// </summary>
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("accounting/security-deposits").WithTags("security-deposits");

        // Create endpoint
        group.MapPost("/", async (CreateSecurityDepositCommand request, ISender mediator, CancellationToken cancellationToken) =>
            {
                var response = await mediator.Send(request, cancellationToken).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("CreateSecurityDeposit")
            .WithSummary("Create a new security deposit")
            .WithDescription("Creates a new security deposit for a member")
            .Produces<CreateSecurityDepositResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}

