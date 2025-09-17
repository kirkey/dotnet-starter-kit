namespace FSH.Starter.Blazor.Client.Models.NavigationMenu;

public class MenuSectionModel
{
    public string Title { get; set; } = string.Empty;
    public string[]? Roles { get; set; }
    public List<MenuSectionItemModel> SectionItems { get; set; } = new();
}

