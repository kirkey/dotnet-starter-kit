using FSH.Starter.Blazor.Client.Components;
using FSH.Starter.Blazor.Infrastructure.Api;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.FluentUI.AspNetCore.Components;
using Shared.Authorization;
using FshValidation = FSH.Starter.Blazor.FluentClient.Components.FshValidation;

namespace FSH.Starter.Blazor.FluentClient.Pages.Auth;

public partial class LoginComponent : ComponentBase
{
    [CascadingParameter]
    public Task<AuthenticationState> AuthState { get; set; } = default!;

    [Inject] protected IToastService ToastService { get; set; } = default!;
    [Inject] protected NavigationManager Navigation { get; set; } = default!;

    protected FshValidation? _customValidation;

    public bool BusySubmitting { get; set; }

    protected readonly TokenGenerationCommand _tokenRequest = new();
    protected string TenantId { get; set; } = string.Empty;
    protected bool _showPassword = false;

    protected override async Task OnInitializedAsync()
    {
        var authState = await AuthState;
        if (authState.User.Identity?.IsAuthenticated is true)
        {
            Navigation.NavigateTo("/");
        }
    }

    protected void TogglePasswordVisibility()
    {
        _showPassword = !_showPassword;
    }

    protected void FillAdministratorCredentials()
    {
        _tokenRequest.Email = TenantConstants.Root.EmailAddress;
        _tokenRequest.Password = TenantConstants.DefaultPassword;
        TenantId = TenantConstants.Root.Id;
    }

    protected async Task SubmitAsync()
    {
        BusySubmitting = true;

        if (await ApiHelper.ExecuteCallGuardedAsync(
            () => authService.LoginAsync(TenantId, _tokenRequest),
            ToastService,
            _customValidation))
        {
            ToastService.ShowSuccess($"Logged in as {_tokenRequest.Email}");
        }

        BusySubmitting = false;
    }
}
