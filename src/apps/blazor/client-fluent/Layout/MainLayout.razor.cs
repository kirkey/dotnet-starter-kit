using FSH.Starter.Blazor.Infrastructure.Api;
using FSH.Starter.Blazor.Infrastructure.Auth;
using FSH.Starter.Blazor.Infrastructure.Preferences;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace FSH.Starter.Blazor.FluentClient.Layout;

public partial class MainLayoutComponent : ComponentBase
{
    [Inject] protected IAuthenticationService AuthService { get; set; } = default!;
    [Inject] protected AuthenticationStateProvider AuthProvider { get; set; } = default!;
    [Inject] protected IClientPreferenceManager ClientPreferences { get; set; } = default!;
    [Inject] protected NavigationManager Navigation { get; set; } = default!;

    [Parameter] public RenderFragment? ChildContent { get; set; }

    protected bool _drawerOpen = true;
    protected bool _isDarkMode;

    protected override async Task OnInitializedAsync()
    {
        var preference = await ClientPreferences.GetPreference();
        if (preference != null)
        {
            _isDarkMode = preference.IsDarkMode;
        }
    }

    protected async Task ToggleDarkMode()
    {
        var preference = await ClientPreferences.GetPreference();
        if (preference != null)
        {
            preference.IsDarkMode = !preference.IsDarkMode;
            _isDarkMode = preference.IsDarkMode;
            await ClientPreferences.SetPreference(preference);
            StateHasChanged();
        }
    }

    protected void Profile()
    {
        Navigation.NavigateTo("/identity/account");
    }

    protected async Task Logout()
    {
        await AuthService.Logout();
        Navigation.NavigateTo("/");
    }
}
