using FSH.Framework.Core.Identity.Users.Abstractions;
using Microsoft.AspNetCore.Http;

namespace FSH.Framework.Infrastructure.Auth;
public class CurrentUserMiddleware(ICurrentUserInitializer currentUserInitializer) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        currentUserInitializer.SetCurrentUser(context.User);
        await next(context).ConfigureAwait(false);
    }
}
