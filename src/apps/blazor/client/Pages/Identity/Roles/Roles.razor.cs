namespace FSH.Starter.Blazor.Client.Pages.Identity.Roles;

public partial class Roles
{
    [CascadingParameter] protected Task<AuthenticationState> AuthState { get; set; } = default!;
    [Inject] protected IAuthorizationService AuthService { get; set; } = default!;
    [Inject] private IClient RolesClient { get; set; } = default!;

    protected EntityClientTableContext<RoleDto, string?, CreateOrUpdateRoleCommand> Context { get; set; } = default!;

    private bool _canViewRoleClaims;

    protected override async Task OnInitializedAsync()
    {
        var state = await AuthState;
        _canViewRoleClaims = await AuthService.HasPermissionAsync(state.User, FshActions.View, FshResources.RoleClaims);

        Context = new EntityClientTableContext<RoleDto, string?, CreateOrUpdateRoleCommand>(
            entityName: "Role",
            entityNamePlural: "Roles",
            entityResource: FshResources.Roles,
            searchAction: FshActions.View,
            fields:
            [
                new EntityField<RoleDto>(dto => dto.Id, "Id"),
                new EntityField<RoleDto>(dto => dto.Name, "Name"),
                new EntityField<RoleDto>(dto => dto.Description, "Description")
            ],
            idFunc: dto => dto.Id,
            loadDataFunc: async () => [.. (await RolesClient.GetRolesEndpointAsync())],
            searchFunc: (searchString, role) =>
                string.IsNullOrWhiteSpace(searchString)
                    || role.Name?.Contains(searchString, StringComparison.OrdinalIgnoreCase) == true
                    || role.Description?.Contains(searchString, StringComparison.OrdinalIgnoreCase) == true,
            createFunc: async role => await RolesClient.CreateOrUpdateRoleEndpointAsync(role),
            updateFunc: async (_, role) => await RolesClient.CreateOrUpdateRoleEndpointAsync(role),
            deleteFunc: async id => await RolesClient.DeleteRoleEndpointAsync(id!),
            hasExtraActionsFunc: () => _canViewRoleClaims,
            canUpdateEntityFunc: e => !FshRoles.IsDefault(e.Name!),
            canDeleteEntityFunc: e => !FshRoles.IsDefault(e.Name!),
            exportAction: string.Empty);
    }

    private void ManagePermissions(string? roleId)
    {
        ArgumentNullException.ThrowIfNull(roleId, nameof(roleId));
        Navigation.NavigateTo($"/identity/roles/{roleId}/permissions");
    }
}
