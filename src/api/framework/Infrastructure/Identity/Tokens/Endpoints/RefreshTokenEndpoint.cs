using FSH.Framework.Core.Identity.Tokens.Features.Refresh;

namespace FSH.Framework.Infrastructure.Identity.Tokens.Endpoints;
public static class RefreshTokenEndpoint
{
    internal static RouteHandlerBuilder MapRefreshTokenEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/refresh", (RefreshTokenCommand request,
            [FromHeader(Name = TenantConstants.Identifier)] string tenant,
            ITokenService service,
            HttpContext context,
            string? deviceType,
            CancellationToken cancellationToken) =>
        {
            string ip = context.GetIpAddress();
            return service.RefreshTokenAsync(request, ip, deviceType, cancellationToken);
        })
        .WithName(nameof(RefreshTokenEndpoint))
        .WithSummary("refresh JWTs")
        .WithDescription("refresh JWTs")
        .AllowAnonymous();
    }
}
