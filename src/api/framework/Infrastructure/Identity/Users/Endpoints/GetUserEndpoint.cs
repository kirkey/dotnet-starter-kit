namespace FSH.Framework.Infrastructure.Identity.Users.Endpoints;
public static class GetUserEndpoint
{
    internal static RouteHandlerBuilder MapGetUserEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", (string id, IUserService service) =>
        {
            return service.GetAsync(id, CancellationToken.None);
        })
        .WithName(nameof(GetUserEndpoint))
        .WithSummary("Get user profile by ID")
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Users))
        .WithDescription("Get another user's profile details by user ID.");
    }
}
