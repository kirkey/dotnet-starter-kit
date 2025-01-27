﻿using FSH.Starter.Blazor.Client.Components;
using FSH.Starter.Blazor.Client.Components.EntityTable;
using FSH.Starter.Blazor.Infrastructure.Api;
using FSH.Starter.Blazor.Infrastructure.Auth;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using Shared.Authorization;

namespace FSH.Starter.Blazor.Client.Pages.Multitenancy;

public partial class Tenants
{
    [Inject]
    private IApiClient ApiClient { get; set; } = default!;
    private string? _searchString;
    protected EntityClientTableContext<TenantViewModel, Guid, CreateTenantCommand> Context { get; set; } = default!;
    private List<TenantViewModel> _tenants = new();
    public EntityTable<TenantViewModel, Guid, CreateTenantCommand> EntityTable { get; set; } = default!;
    [CascadingParameter]
    protected Task<AuthenticationState> AuthState { get; set; } = default!;
    [Inject]
    protected IAuthorizationService AuthService { get; set; } = default!;

    private bool _canUpgrade;
    private bool _canModify;

    protected override async Task OnInitializedAsync()
    {
        Context = new EntityClientTableContext<TenantViewModel, Guid, CreateTenantCommand>(
            entityName: "Tenant",
            entityNamePlural: "Tenants",
            entityResource: FshResources.Tenants,
            searchAction: FshActions.View,
            deleteAction: string.Empty,
            updateAction: string.Empty,
            fields: new List<EntityField<TenantViewModel>>
            {
                new(tenant => tenant.Id, "Id"),
                new(tenant => tenant.Name, "Name"),
                new(tenant => tenant.AdminEmail, "Admin Email"),
                new(tenant => tenant.ValidUpto.ToString("MMM dd, yyyy"), "Valid Upto"),
                new(tenant => tenant.IsActive, "Active", Type: typeof(bool))
            },
            loadDataFunc: async () => _tenants = (await ApiClient.GetTenantsEndpointAsync().ConfigureAwait(false)).Adapt<List<TenantViewModel>>(),
            searchFunc: (searchString, tenantDto) =>
                string.IsNullOrWhiteSpace(searchString)
                    || tenantDto.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase),
            createFunc: tenant => ApiClient.CreateTenantEndpointAsync(tenant.Adapt<CreateTenantCommand>()),
            hasExtraActionsFunc: () => true,
            exportAction: string.Empty);

        var state = await AuthState.ConfigureAwait(false);
        _canUpgrade = await AuthService.HasPermissionAsync(state.User, FshActions.UpgradeSubscription, FshResources.Tenants).ConfigureAwait(false);
        _canModify = await AuthService.HasPermissionAsync(state.User, FshActions.Update, FshResources.Tenants).ConfigureAwait(false);
    }

    private void ViewTenantDetails(string id)
    {
        var tenant = _tenants.First(f => f.Id == id);
        tenant.ShowDetails = !tenant.ShowDetails;
        foreach (var otherTenants in _tenants.Except(new[] { tenant }))
        {
            otherTenants.ShowDetails = false;
        }
    }

    private async Task ViewUpgradeSubscriptionModalAsync(string id)
    {
        var tenant = _tenants.First(f => f.Id == id);
        var parameters = new DialogParameters
        {
            {
                nameof(UpgradeSubscriptionModal.Request),
                new UpgradeSubscriptionCommand
                {
                    Tenant = tenant.Id,
                    ExtendedExpiryDate = tenant.ValidUpto
                }
            }
        };
        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, BackdropClick = false };
        var dialog = DialogService.Show<UpgradeSubscriptionModal>("Upgrade Subscription", parameters, options);
        var result = await dialog.Result.ConfigureAwait(false);
        if (!result.Canceled)
        {
            await EntityTable.ReloadDataAsync().ConfigureAwait(false);
        }
    }

    private async Task DeactivateTenantAsync(string id)
    {
        if (await ApiHelper.ExecuteCallGuardedAsync(
                () => ApiClient.DisableTenantEndpointAsync(id),
                Toast, Navigation,
                null,
                "Tenant Deactivated.").ConfigureAwait(false) is not null)
        {
            await EntityTable.ReloadDataAsync().ConfigureAwait(false);
        }
    }

    private async Task ActivateTenantAsync(string id)
    {
        if (await ApiHelper.ExecuteCallGuardedAsync(
                () => ApiClient.ActivateTenantEndpointAsync(id),
                Toast, Navigation,
                null,
                "Tenant Activated.").ConfigureAwait(false) is not null)
        {
            await EntityTable.ReloadDataAsync().ConfigureAwait(false);
        }
    }

    public class TenantViewModel : TenantDetail
    {
        public bool ShowDetails { get; set; }
    }
}
