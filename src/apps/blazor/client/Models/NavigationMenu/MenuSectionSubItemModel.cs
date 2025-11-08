namespace FSH.Starter.Blazor.Client.Models.NavigationMenu;

/// <summary>
/// Represents a sub-menu item that appears under a parent menu item.
/// These are the actual navigable items within expandable menu groups.
/// </summary>
public class MenuSectionSubItemModel
{
    /// <summary>
    /// Gets or sets the display title of the sub-menu item.
    /// </summary>
    public string Title { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the icon to display alongside the sub-menu item title.
    /// Uses Material Design icons from MudBlazor.
    /// </summary>
    public string? Icon { get; set; }
    
    /// <summary>
    /// Gets or sets the URL to navigate to when the sub-menu item is clicked.
    /// </summary>
    public string Href { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets whether this item is a group header/divider (non-clickable visual separator).
    /// </summary>
    public bool IsGroupHeader { get; set; }
    
    /// <summary>
    /// Gets or sets the target window/frame for navigation (e.g., "_blank" for new window).
    /// </summary>
    public string? Target { get; set; }
    
    /// <summary>
    /// Gets or sets a value indicating whether the sub-menu item is disabled.
    /// Disabled items cannot be clicked and are visually indicated as inactive.
    /// </summary>
    public bool Disabled { get; set; }
    
    /// <summary>
    /// Gets or sets the development status of the page/feature.
    /// - None: Stable, production-ready feature
    /// - Completed: Fully implemented and tested
    /// - InProgress: Available but may contain bugs, shown with warning indicators
    /// - ComingSoon: Not yet available, item is disabled with coming soon indicator
    /// </summary>
    public PageStatus PageStatus { get; set; } = PageStatus.None;
    
    /// <summary>
    /// Gets or sets the roles required to access this sub-menu item.
    /// If null or empty, the item is accessible to all authenticated users.
    /// </summary>
    public string[]? Roles { get; set; }
    
    /// <summary>
    /// Gets or sets the specific permission required to access this sub-menu item.
    /// Used for granular permission checking.
    /// </summary>
    public string? Permission { get; set; }
    
    /// <summary>
    /// Gets or sets the permission action required to access this sub-menu item.
    /// Used in conjunction with Resource for fine-grained permission control.
    /// </summary>
    public string? Action { get; set; }
    
    /// <summary>
    /// Gets or sets the resource that the permission action applies to.
    /// Used in conjunction with Action for fine-grained permission control.
    /// </summary>
    public string? Resource { get; set; }
}
