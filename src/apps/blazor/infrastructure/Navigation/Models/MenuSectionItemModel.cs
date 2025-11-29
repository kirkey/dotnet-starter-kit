namespace FSH.Starter.Blazor.Infrastructure.Navigation.Models;

/// <summary>
/// Represents a navigation menu item that can be either a standalone link or a parent containing sub-items.
/// Supports different page statuses to indicate development state and user access levels.
/// </summary>
public class MenuSectionItemModel
{
    /// <summary>
    /// Gets or sets the display title of the menu item.
    /// </summary>
    public string Title { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the icon to display alongside the menu item title.
    /// Uses Material Design icons from MudBlazor.
    /// </summary>
    public string? Icon { get; set; }
    
    /// <summary>
    /// Gets or sets the URL to navigate to when the menu item is clicked.
    /// Not applicable for parent items that contain sub-items.
    /// </summary>
    public string? Href { get; set; }
    
    /// <summary>
    /// Gets or sets the target window/frame for navigation (e.g., "_blank" for new window).
    /// </summary>
    public string? Target { get; set; }
    
    /// <summary>
    /// Gets or sets a value indicating whether the menu item is disabled.
    /// Disabled items cannot be clicked and are visually indicated as inactive.
    /// </summary>
    public bool Disabled { get; set; }
    
    /// <summary>
    /// Gets or sets a value indicating whether this item is a parent containing sub-items.
    /// Parent items expand to show their MenuItems collection.
    /// </summary>
    public bool IsParent { get; set; }
    
    /// <summary>
    /// Gets or sets the collection of sub-menu items when this is a parent item.
    /// Only applicable when IsParent is true.
    /// </summary>
    public List<MenuSectionSubItemModel>? MenuItems { get; set; }
    
    /// <summary>
    /// Gets or sets the roles required to access this menu item.
    /// If null or empty, the item is accessible to all authenticated users.
    /// </summary>
    public string[]? Roles { get; set; }
    
    /// <summary>
    /// Gets or sets the permission action required to access this menu item.
    /// Used in conjunction with Resource for fine-grained permission control.
    /// </summary>
    public string? Action { get; set; }
    
    /// <summary>
    /// Gets or sets the resource that the permission action applies to.
    /// Used in conjunction with Action for fine-grained permission control.
    /// </summary>
    public string? Resource { get; set; }
    
    /// <summary>
    /// Gets or sets the development status of the page/feature.
    /// - None: Stable, production-ready feature
    /// - Completed: Fully implemented and tested
    /// - InProgress: Available but may contain bugs, shown with warning indicators
    /// - ComingSoon: Not yet available, item is disabled with coming soon indicator
    /// </summary>
    public PageStatus PageStatus { get; set; } = PageStatus.None;
}

