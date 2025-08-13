using FSH.Starter.Blazor.Infrastructure.Preferences;
using FSH.Starter.Blazor.Infrastructure.Themes;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.AspNetCore.Components;

namespace FSH.Starter.Blazor.FluentClient.Layout;

public partial class BaseLayoutComponent : ComponentBase
{
    [Inject] protected IClientPreferenceManager ClientPreferences { get; set; } = default!;
    [Inject] protected IToastService ToastService { get; set; } = default!;
    [Inject] protected NavigationManager Navigation { get; set; } = default!;
    private ClientPreference? _themePreference;
    private FluentTheme _currentTheme = new FluentTheme();
    private bool _themeDrawerOpen;
    private bool _rightToLeft;
    private bool _isDarkMode;
    private DesignThemeModes _designThemeMode = DesignThemeModes.System;

    protected override async Task OnInitializedAsync()
    {
        _themePreference = await ClientPreferences.GetPreference() as ClientPreference;
        if (_themePreference == null) _themePreference = new ClientPreference();
        SetCurrentTheme(_themePreference);

        // ToastService.ShowInfo("Like this project? Star us on Github!", 
        //     options => 
        //     {
        //         options.Title = "Support";
        //         options.Intent = ToastIntent.Info;
        //         options.PrimaryAction = "Star us on Github!";
        //         options.PrimaryActionClick = (toast) =>
        //         {
        //             Navigation.NavigateTo("https://github.com/fullstackhero/dotnet-starter-kit");
        //             return Task.CompletedTask;
        //         };
        //     });
    }

    private async Task ToggleDarkLightMode(bool isDarkMode)
    {
        if (_themePreference is not null)
        {
            _themePreference.IsDarkMode = isDarkMode;
            await ThemePreferenceChanged(_themePreference);
        }
    }

    private async Task ThemePreferenceChanged(ClientPreference themePreference)
    {
        SetCurrentTheme(themePreference);
        await ClientPreferences.SetPreference(themePreference);
    }

    private void SetCurrentTheme(ClientPreference themePreference)
    {
        _isDarkMode = themePreference.IsDarkMode;
        _designThemeMode = _isDarkMode ? DesignThemeModes.Dark : DesignThemeModes.Light;
        _rightToLeft = themePreference.IsRtl;
        
        // Create Fluent theme from preference
        _currentTheme = new FluentTheme
        {
            Primary = themePreference.PrimaryColor ?? "#0078d4",
            Secondary = themePreference.SecondaryColor ?? "#ff6900"
        };
        
        StateHasChanged();
    }

    void ToggleThemeDrawer()
    {
        _themeDrawerOpen = !_themeDrawerOpen;
    }
}

public class FluentTheme
{
    public string Primary { get; set; } = "#0078d4";
    public string Secondary { get; set; } = "#ff6900";
}
