using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.Blazor.Infrastructure.Auth.Jwt;

public class AccessTokenProviderAccessor(IServiceProvider provider) : IAccessTokenProviderAccessor
{
    private readonly IServiceProvider _provider = provider;
    private IAccessTokenProvider? _tokenProvider;

    public IAccessTokenProvider TokenProvider =>
        _tokenProvider ??= _provider.GetRequiredService<IAccessTokenProvider>();
}