using FSH.Starter.Blazor.Infrastructure.Preferences;
using FSH.Starter.Blazor.Infrastructure.Themes;
using MudBlazor;
using FSH.Starter.Blazor.Infrastructure.Connectivity;
using FSH.Starter.Blazor.Infrastructure.Offline;
using FSH.Starter.Blazor.Infrastructure.Features;
using FSH.Starter.Blazor.Infrastructure.Session;
using FSH.Starter.Blazor.Infrastructure.Auth;
using Microsoft.JSInterop;
using FSH.Starter.Blazor.Infrastructure.Palette;
using FSH.Starter.Blazor.Infrastructure.Localization;
using FSH.Starter.Blazor.Infrastructure.Sync;

namespace FSH.Starter.Blazor.Client.Layout;

public partial class BaseLayout : IDisposable
{
    private ClientPreference? _themePreference;
    private readonly MudTheme _currentTheme = new FshTheme();
    private bool _themeDrawerOpen;
    private bool _isDarkMode;
    private string? _idleWarning;
    private bool _showIdleWarning;

    [Microsoft.AspNetCore.Components.Inject] private INetworkStatusService Network { get; set; } = default!;
    [Microsoft.AspNetCore.Components.Inject] private IOfflineRequestQueue OfflineQueue { get; set; } = default!;
    [Microsoft.AspNetCore.Components.Inject] private HttpClient Http { get; set; } = default!;
    [Microsoft.AspNetCore.Components.Inject] private IShortcutService Shortcuts { get; set; } = default!;
    [Microsoft.AspNetCore.Components.Inject] private IIdleTimerService Idle { get; set; } = default!;
    [Microsoft.AspNetCore.Components.Inject] private IAuthenticationService Auth { get; set; } = default!;
    [Microsoft.AspNetCore.Components.Inject] private IJSRuntime JS { get; set; } = default!;
    [Microsoft.AspNetCore.Components.Inject] private ICommandPaletteService Palette { get; set; } = default!;
    [Microsoft.AspNetCore.Components.Inject] private ILocalizationService L10n { get; set; } = default!;
    [Microsoft.AspNetCore.Components.Inject] private IBroadcastSyncService Broadcaster { get; set; } = default!;

    private Components.Common.ShortcutHelp? _shortcutHelp;
    private Components.Common.CommandPalette? _commandPalette;

    private bool _online;
    private int _pending;

    protected override async Task OnInitializedAsync()
    {
        _themePreference = await ClientPreferences.GetPreference() as ClientPreference;
        if (_themePreference == null) _themePreference = new ClientPreference();
        SetCurrentTheme(_themePreference);

        await Network.InitializeAsync();
        _ = Network.IsOnline;
        _ = OfflineQueue.PendingCount;
        Network.StatusChanged += OnNetworkStatusChanged;

        // Initialize shortcuts
        Shortcuts.Register("Ctrl+/", "show-help", "Show shortcut help");
        Shortcuts.Register("Ctrl+K", "command-palette", "Open command palette (placeholder)");
        await Shortcuts.InitializeAsync(JS);
        Shortcuts.Triggered += OnShortcut;

        // Start idle timer (15 min for demo, warn at 60s)
        await Idle.StartAsync(timeoutSeconds: 900, warningSeconds: 60);
        Idle.Warning += OnIdleWarning;
        Idle.TimedOut += OnIdleTimeout;

        await L10n.InitializeAsync();
        Broadcaster.Initialize();
        Palette.RegisterCommand(new("open-settings","Open Settings","Navigation", () => Navigation.NavigateTo("/app/settings")));
        Palette.RegisterCommand(new("logout","Logout","Account", () =>
        {
            try { Auth.LogoutAsync().GetAwaiter().GetResult(); Navigation.NavigateTo("/login", forceLoad:true); }
            catch { /* handle/log error */ }
        }));
        Palette.RegisterCommand(new("toggle-theme","Toggle Theme","UI", () =>
        {
            try { ToggleDarkLightMode(!_isDarkMode).GetAwaiter().GetResult(); }
            catch { /* handle/log error */ }
        }));

        const string GitHubUrl = "https://github.com/fullstackhero/dotnet-starter-kit";
        Toast.Add("Like this project? ", Severity.Info, config =>
        {
            config.BackgroundBlurred = true;
            config.Icon = Icons.Custom.Brands.GitHub;
            config.Action = "Star us on Github!";
            config.ActionColor = Color.Info;
            config.OnClick = _ =>
            {
                Navigation.NavigateTo(GitHubUrl);
                return Task.CompletedTask;
            };
        });
    }

    private void OnShortcut(string commandId)
    {
        switch (commandId)
        {
            case "show-help":
                _shortcutHelp?.Open();
                break;
            case "command-palette":
                _commandPalette?.Open();
                break;
        }
    }

    private void OnIdleWarning(TimeSpan remaining)
    {
        _idleWarning = $"Session will timeout in {remaining.Seconds} seconds";
        if (!_showIdleWarning)
        {
            _showIdleWarning = true;
            Toast.Add(_idleWarning, Severity.Warning, cfg => cfg.RequireInteraction = true);
        }
    }

    private async void OnIdleTimeout()
    {
        Toast.Add("Session timed out. Logging out...", Severity.Error);
        await Auth.LogoutAsync();
        Navigation.NavigateTo("/login", forceLoad:true);
    }

    private void OnNetworkStatusChanged(bool isOnline)
    {
        _online = isOnline;
        if (isOnline)
        {
            _ = OfflineQueue.FlushAsync(async qr =>
            {
                try
                {
                    var msg = new HttpRequestMessage(new HttpMethod(qr.Method), qr.Url);
                    if (qr.Body != null)
                    {
                        msg.Content = new StringContent(qr.Body, System.Text.Encoding.UTF8,
                            qr.Headers.TryGetValue("Content-Type", out var ct) ? ct : "application/json");
                    }
                    foreach (var h in qr.Headers)
                    {
                        if (h.Key.Equals("Content-Type", StringComparison.OrdinalIgnoreCase)) continue;
                        if (h.Key.Equals("Authorization", StringComparison.OrdinalIgnoreCase)) continue;
                        msg.Headers.TryAddWithoutValidation(h.Key, h.Value);
                    }
                    var resp = await Http.SendAsync(msg);
                    return resp.IsSuccessStatusCode;
                }
                catch
                {
                    return false;
                }
            });
        }
        StateHasChanged();
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
        _currentTheme.PaletteLight.Primary = themePreference.PrimaryColor;
        _currentTheme.PaletteLight.Secondary = themePreference.SecondaryColor;
        _currentTheme.PaletteDark.Primary = themePreference.PrimaryColor;
        _currentTheme.PaletteDark.Secondary = themePreference.SecondaryColor;
        _currentTheme.LayoutProperties.DefaultBorderRadius = $"{themePreference.BorderRadius}px";
        _currentTheme.LayoutProperties.DefaultBorderRadius = $"{themePreference.BorderRadius}px";
    }

    public void Dispose()
    {
        Network.StatusChanged -= OnNetworkStatusChanged;
        Shortcuts.Triggered -= OnShortcut;
        Idle.Warning -= OnIdleWarning;
        Idle.TimedOut -= OnIdleTimeout;
        Broadcaster.DisposeAsync().AsTask().ConfigureAwait(false);
        GC.SuppressFinalize(this);
    }
}
