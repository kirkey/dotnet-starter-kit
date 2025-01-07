using FSH.Starter.Blazor.Infrastructure.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Shared.Authorization;

namespace FSH.Starter.Blazor.Client.Layout;

public partial class NavMenu
{
    [CascadingParameter]
    protected Task<AuthenticationState> AuthState { get; set; } = default!;
    [Inject]
    protected IAuthorizationService AuthService { get; set; } = default!;

    private bool _canViewHangfire;
    private bool _canViewDashboard;
    private bool _canViewRoles;
    private bool _canViewUsers;
    private bool _canViewProducts;
    private bool _canViewBrands;
    private bool _canViewTodos;
    private bool _canViewTenants;
    private bool _canViewAuditTrails;
    private bool CanViewAdministrationGroup => _canViewUsers || _canViewRoles || _canViewTenants;

    protected override async Task OnParametersSetAsync()
    {
        var user = (await AuthState.ConfigureAwait(false)).User;
        _canViewHangfire = await AuthService.HasPermissionAsync(user, FshActions.View, FshResources.Hangfire).ConfigureAwait(false);
        _canViewDashboard = await AuthService.HasPermissionAsync(user, FshActions.View, FshResources.Dashboard).ConfigureAwait(false);
        _canViewRoles = await AuthService.HasPermissionAsync(user, FshActions.View, FshResources.Roles).ConfigureAwait(false);
        _canViewUsers = await AuthService.HasPermissionAsync(user, FshActions.View, FshResources.Users).ConfigureAwait(false);
        _canViewProducts = await AuthService.HasPermissionAsync(user, FshActions.View, FshResources.Products).ConfigureAwait(false);
        _canViewBrands = await AuthService.HasPermissionAsync(user, FshActions.View, FshResources.Brands).ConfigureAwait(false);
        _canViewTodos = await AuthService.HasPermissionAsync(user, FshActions.View, FshResources.Todos).ConfigureAwait(false);
        _canViewTenants = await AuthService.HasPermissionAsync(user, FshActions.View, FshResources.Tenants).ConfigureAwait(false);
        _canViewAuditTrails = await AuthService.HasPermissionAsync(user, FshActions.View, FshResources.AuditTrails).ConfigureAwait(false);
    }
}
