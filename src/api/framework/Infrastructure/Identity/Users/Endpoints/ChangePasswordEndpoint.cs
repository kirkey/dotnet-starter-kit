using FluentValidation.Results;
using FSH.Framework.Core.Identity.Users.Features.ChangePassword;

namespace FSH.Framework.Infrastructure.Identity.Users.Endpoints;
public static class ChangePasswordEndpoint
{
    internal static RouteHandlerBuilder MapChangePasswordEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/change-password", async (ChangePasswordCommand command,
            HttpContext context,
            IOptions<OriginOptions> settings,
            IValidator<ChangePasswordCommand> validator,
            IUserService userService,
            CancellationToken cancellationToken) =>
        {
            ValidationResult result = await validator.ValidateAsync(command, cancellationToken).ConfigureAwait(false);
            if (!result.IsValid)
            {
                return Results.ValidationProblem(result.ToDictionary());
            }

            if (context.User.GetUserId() is not { } userId || string.IsNullOrEmpty(userId))
            {
                return Results.BadRequest();
            }

            await userService.ChangePasswordAsync(command, userId).ConfigureAwait(false);
            return Results.Ok("password reset email sent");
        })
        .WithName(nameof(ChangePasswordEndpoint))
        .WithSummary("Changes password")
        .WithDescription("Change password");
    }

}
