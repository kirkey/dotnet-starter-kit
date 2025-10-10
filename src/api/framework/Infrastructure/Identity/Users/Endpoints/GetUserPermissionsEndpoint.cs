namespace FSH.Framework.Infrastructure.Identity.Users.Endpoints;

public static class GetUserPermissionsEndpoint
{
    internal static RouteHandlerBuilder MapGetCurrentUserPermissionsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/permissions", async (ClaimsPrincipal user, IUserService service, CancellationToken cancellationToken) =>
        {
            if (user.GetUserId() is not { } userId || string.IsNullOrEmpty(userId))
            {
                throw new UnauthorizedException();
            }

            return await service.GetPermissionsAsync(userId, cancellationToken).ConfigureAwait(false);
        })
        .WithName(nameof(GetUserPermissionsEndpoint))
        .WithSummary("Get current user permissions")
        .WithDescription("Get current user permissions");
    }
}
