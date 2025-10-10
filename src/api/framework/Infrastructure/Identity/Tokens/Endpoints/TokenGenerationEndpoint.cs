using FSH.Framework.Core.Identity.Tokens.Features.Generate;

namespace FSH.Framework.Infrastructure.Identity.Tokens.Endpoints;
public static class TokenGenerationEndpoint
{
    internal static RouteHandlerBuilder MapTokenGenerationEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/", (TokenGenerationCommand request,
            [FromHeader(Name = TenantConstants.Identifier)] string tenant,
            ITokenService service,
            HttpContext context,
            string? deviceType,
            CancellationToken cancellationToken) =>
        {
            string ip = context.GetIpAddress();
            return service.GenerateTokenAsync(request, ip, deviceType, cancellationToken);
        })
        .WithName(nameof(TokenGenerationEndpoint))
        .WithSummary("generate JWTs")
        .WithDescription("generate JWTs")
        .AllowAnonymous();
    }
}
