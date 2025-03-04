using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication.Internal;
using System.Net.Http.Headers;

namespace FSH.Starter.Blazor.Infrastructure.Auth.Jwt;
public class JwtAuthenticationHeaderHandler(IAccessTokenProviderAccessor tokenProviderAccessor, NavigationManager navigation) : DelegatingHandler
{
    private readonly IAccessTokenProviderAccessor _tokenProviderAccessor = tokenProviderAccessor;
    private readonly NavigationManager _navigation = navigation;

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        // skip token endpoints
        if (request.RequestUri?.AbsolutePath.Contains("/token") is not true)
        {
            if (await _tokenProviderAccessor.TokenProvider.GetAccessTokenAsync().ConfigureAwait(false) is string token)
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            else
            {
                _navigation.NavigateTo("/login");
            }
        }

        return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
    }
}