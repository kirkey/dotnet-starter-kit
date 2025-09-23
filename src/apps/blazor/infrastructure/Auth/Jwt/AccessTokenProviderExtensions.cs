namespace FSH.Starter.Blazor.Infrastructure.Auth.Jwt;

public static class AccessTokenProviderExtensions
{
    public static async Task<string?> GetAccessTokenAsync(this IAccessTokenProvider tokenProvider) =>
        (await tokenProvider.RequestAccessToken().ConfigureAwait(false))
            .TryGetToken(out var token)
                ? token.Value
                : null;
}
