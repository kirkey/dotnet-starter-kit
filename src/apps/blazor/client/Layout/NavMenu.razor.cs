using FSH.Starter.Blazor.Infrastructure.Navigation;
using FSH.Starter.Blazor.Infrastructure.Navigation.Models;

namespace FSH.Starter.Blazor.Client.Layout;
 
/// <summary>
/// Navigation menu component that renders the application's main navigation structure.
/// Handles menu filtering based on user roles and permissions, and provides special handling for ComingSoon items.
/// </summary>
public partial class NavMenu
{
    [CascadingParameter]
    protected Task<AuthenticationState> AuthState { get; set; } = null!;
    [Inject]
    protected IAuthorizationService AuthService { get; set; } = null!;
    [Inject]
    private IMenuService MenuService { get; set; } = null!;

    private List<MenuSectionModel> _sections = [];

    /// <summary>
    /// Initializes the navigation menu by filtering sections and items based on user roles and permissions.
    /// </summary>
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

                if (item is { IsParent: true, MenuItems: not null })
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
                        // Check if item has authorization requirements and if user lacks permission
                        bool hasPermission = await HasPermissionAsync(item.Action, item.Resource).ConfigureAwait(false);
                        bool isDisabled = !hasPermission && (!string.IsNullOrWhiteSpace(item.Action) || !string.IsNullOrWhiteSpace(item.Resource));

                        filteredSection.SectionItems.Add(new MenuSectionItemModel
                        {
                            Title = item.Title,
                            Icon = item.Icon,
                            IsParent = true,
                            MenuItems = filteredSubs,
                            PageStatus = item.PageStatus,
                            Disabled = isDisabled || item.PageStatus == PageStatus.ComingSoon,
                            Action = item.Action,
                            Resource = item.Resource
                        });
                    }
                }
                else
                {
                    bool hasPermission = await HasPermissionAsync(item.Action, item.Resource).ConfigureAwait(false);
                    bool isDisabled = !hasPermission && (!string.IsNullOrWhiteSpace(item.Action) || !string.IsNullOrWhiteSpace(item.Resource));
                    
                    var menuItem = new MenuSectionItemModel
                    {
                        Title = item.Title,
                        Icon = item.Icon,
                        Href = item.Href,
                        Target = item.Target,
                        Disabled = isDisabled || item.PageStatus == PageStatus.ComingSoon,
                        IsParent = item.IsParent,
                        MenuItems = item.MenuItems,
                        Roles = item.Roles,
                        Action = item.Action,
                        Resource = item.Resource,
                        PageStatus = item.PageStatus
                    };
                    
                    filteredSection.SectionItems.Add(menuItem);
                }
            }

            if (filteredSection.SectionItems.Count > 0)
            {
                result.Add(filteredSection);
            }
        }

        _sections = result;
    }

    /// <summary>
    /// Determines if a menu item should be disabled based on its PageStatus.
    /// InProgress items remain accessible to allow users to test new features.
    /// </summary>
    /// <param name="item">The menu item to check.</param>
    /// <returns>True if the item should be disabled, false otherwise.</returns>
    private bool IsItemDisabled(MenuSectionItemModel item)
    {
        return item.PageStatus == PageStatus.ComingSoon;
    }

    /// <summary>
    /// Determines if a sub-menu item should be disabled based on its PageStatus.
    /// InProgress items remain accessible to allow users to test new features.
    /// </summary>
    /// <param name="subItem">The sub-menu item to check.</param>
    /// <returns>True if the sub-item should be disabled, false otherwise.</returns>
    private bool IsSubItemDisabled(MenuSectionSubItemModel subItem)
    {
        return subItem.PageStatus == PageStatus.ComingSoon;
    }
}
