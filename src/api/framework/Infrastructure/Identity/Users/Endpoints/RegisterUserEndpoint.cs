using FSH.Framework.Core.Identity.Users.Features.RegisterUser;

namespace FSH.Framework.Infrastructure.Identity.Users.Endpoints;
public static class RegisterUserEndpoint
{
    internal static RouteHandlerBuilder MapRegisterUserEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/register", (RegisterUserCommand request,
            IUserService service,
            HttpContext context,
            CancellationToken cancellationToken) =>
        {
            var origin = $"{context.Request.Scheme}://{context.Request.Host.Value}{context.Request.PathBase.Value}";
            return service.RegisterAsync(request, origin, cancellationToken);
        })
        .WithName(nameof(RegisterUserEndpoint))
        .WithSummary("register user")
        .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Users))
        .WithDescription("register user");
    }
}
