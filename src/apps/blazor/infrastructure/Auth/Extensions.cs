namespace FSH.Starter.Blazor.Infrastructure.Auth;
public static class Extensions
{
    public static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration config)
    {
        services.AddScoped<AuthenticationStateProvider, JwtAuthenticationService>()
                .AddScoped(sp => (IAuthenticationService)sp.GetRequiredService<AuthenticationStateProvider>())
                .AddScoped(sp => (IAccessTokenProvider)sp.GetRequiredService<AuthenticationStateProvider>())
                .AddScoped<IAccessTokenProviderAccessor, AccessTokenProviderAccessor>()
                .AddScoped<JwtAuthenticationHeaderHandler>();

        services.AddAuthorizationCore(RegisterPermissionClaims);
        services.AddCascadingAuthenticationState();
        return services;
    }


    private static void RegisterPermissionClaims(AuthorizationOptions options)
    {
        foreach (var permission in FshPermissions.All.Select(p => p.Name))
        {
            options.AddPolicy(permission, policy => policy.RequireClaim(FshClaims.Permission, permission));
        }
    }
}
