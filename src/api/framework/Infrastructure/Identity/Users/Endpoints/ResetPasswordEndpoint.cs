using FluentValidation.Results;
using FSH.Framework.Core.Identity.Users.Features.ResetPassword;

namespace FSH.Framework.Infrastructure.Identity.Users.Endpoints;

public static class ResetPasswordEndpoint
{
    internal static RouteHandlerBuilder MapResetPasswordEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/reset-password", async (ResetPasswordCommand command, [FromHeader(Name = TenantConstants.Identifier)] string tenant, IValidator<ResetPasswordCommand> validator, IUserService userService, CancellationToken cancellationToken) =>
        {
            ValidationResult result = await validator.ValidateAsync(command, cancellationToken).ConfigureAwait(false);
            if (!result.IsValid)
            {
                return Results.ValidationProblem(result.ToDictionary());
            }

            await userService.ResetPasswordAsync(command, cancellationToken).ConfigureAwait(false);
            return Results.Ok("Password has been reset.");
        })
        .WithName(nameof(ResetPasswordEndpoint))
        .WithSummary("Reset password")
        .WithDescription("Resets the password using the token and new password provided.")
        .AllowAnonymous();
    }

}
