using Accounting.Application.Checks.Print.v1;

namespace Accounting.Infrastructure.Endpoints.Checks.v1;

/// <summary>
/// Endpoint for marking a check as printed.
/// </summary>
public static class CheckPrintEndpoint
{
    internal static RouteHandlerBuilder MapCheckPrintEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/print", async (CheckPrintCommand command, ISender mediator) =>
            {
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(CheckPrintEndpoint))
            .WithSummary("Mark check as printed")
            .WithDescription("Mark a check as printed for audit trail purposes")
            .Produces<CheckPrintResponse>()
            .RequirePermission("Permissions.Accounting.Update")
            .MapToApiVersion(1);
    }
}

