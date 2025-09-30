using FSH.Starter.Blazor.Client.Models.NavigationMenu;

namespace FSH.Starter.Blazor.Client.Services.Navigation;

public interface IMenuService
{
    IEnumerable<MenuSectionModel> Features { get; }
}
