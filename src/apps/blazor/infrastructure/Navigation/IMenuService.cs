using FSH.Starter.Blazor.Infrastructure.Navigation.Models;

namespace FSH.Starter.Blazor.Infrastructure.Navigation;

public interface IMenuService
{
    IEnumerable<MenuSectionModel> Features { get; }
}
