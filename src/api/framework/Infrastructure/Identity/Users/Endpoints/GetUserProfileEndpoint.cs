namespace FSH.Framework.Infrastructure.Identity.Users.Endpoints;

public static class GetUserProfileEndpoint
{
    internal static RouteHandlerBuilder MapGetMeEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/profile", async (ClaimsPrincipal user, IUserService service, CancellationToken cancellationToken) =>
        {
            if (user.GetUserId() is not { } userId || string.IsNullOrEmpty(userId))
            {
                throw new UnauthorizedException();
            }

            return await service.GetAsync(userId, cancellationToken).ConfigureAwait(false);
        })
        .WithName(nameof(GetUserProfileEndpoint))
        .WithSummary("Get current user information based on token")
        .WithDescription("Get current user information based on token");
    }
}
