using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.Blazor.Infrastructure.Auth.Jwt;

public class AccessTokenProviderAccessor(IServiceProvider provider) : IAccessTokenProviderAccessor
{
    private IAccessTokenProvider? _tokenProvider;

    public IAccessTokenProvider TokenProvider =>
        _tokenProvider ??= provider.GetRequiredService<IAccessTokenProvider>();
}