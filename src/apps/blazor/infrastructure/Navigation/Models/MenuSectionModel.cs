namespace FSH.Starter.Blazor.Infrastructure.Navigation.Models;

/// <summary>
/// Represents a navigation menu section containing a collection of menu items.
/// Used to group related navigation items under a common section title.
/// </summary>
public class MenuSectionModel
{
    /// <summary>
    /// Gets or sets the title of the menu section.
    /// This title is displayed as a header above the section items.
    /// </summary>
    public string Title { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the collection of menu items within this section.
    /// Contains the actual navigable menu items and their properties.
    /// </summary>
    public List<MenuSectionItemModel> SectionItems { get; set; } = [];
    
    /// <summary>
    /// Gets or sets the roles required to access this menu section.
    /// If null or empty, the section is accessible to all users.
    /// </summary>
    public string[]? Roles { get; set; }
}
