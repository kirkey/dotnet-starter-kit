namespace FSH.Starter.Blazor.Client.Layout;

using FSH.Starter.Blazor.Client.Models.NavigationMenu;
using FSH.Starter.Blazor.Client.Services.Navigation;

public partial class NavMenu
{
    [CascadingParameter]
    protected Task<AuthenticationState> AuthState { get; set; } = default!;
    [Inject]
    protected IAuthorizationService AuthService { get; set; } = default!;
    [Inject]
    private IMenuService MenuService { get; set; } = default!;

    private List<MenuSectionModel> _sections = new();

    protected override async Task OnParametersSetAsync()
    {
        var user = (await AuthState).User;

        bool HasAnyRole(string[]? roles)
            => roles is null || roles.Length == 0 || roles.Any(user.IsInRole);

        async Task<bool> HasPermissionAsync(string? action, string? resource)
        {
            if (string.IsNullOrWhiteSpace(action) || string.IsNullOrWhiteSpace(resource))
                return true;
            return await AuthService.HasPermissionAsync(user, action, resource).ConfigureAwait(false);
        }

        var result = new List<MenuSectionModel>();
        foreach (var section in MenuService.Features)
        {
            if (!HasAnyRole(section.Roles)) continue;

            var filteredSection = new MenuSectionModel { Title = section.Title };

            foreach (var item in section.SectionItems)
            {
                if (!HasAnyRole(item.Roles)) continue;

                if (item.IsParent && item.MenuItems is not null)
                {
                    var filteredSubs = new List<MenuSectionSubItemModel>();
                    foreach (var sub in item.MenuItems)
                    {
                        if (!HasAnyRole(sub.Roles)) continue;
                        if (!await HasPermissionAsync(sub.Action, sub.Resource).ConfigureAwait(false)) continue;
                        filteredSubs.Add(sub);
                    }

                    if (filteredSubs.Count > 0)
                    {
                        filteredSection.SectionItems.Add(new MenuSectionItemModel
                        {
                            Title = item.Title,
                            Icon = item.Icon,
                            IsParent = true,
                            MenuItems = filteredSubs
                        });
                    }
                }
                else
                {
                    if (!await HasPermissionAsync(item.Action, item.Resource).ConfigureAwait(false)) continue;
                    filteredSection.SectionItems.Add(item);
                }
            }

            if (filteredSection.SectionItems.Count > 0)
            {
                result.Add(filteredSection);
            }
        }

        _sections = result;
    }
}
