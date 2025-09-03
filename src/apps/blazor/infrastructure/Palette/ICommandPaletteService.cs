namespace FSH.Starter.Blazor.Infrastructure.Palette;
public interface ICommandPaletteService
{
    void RegisterCommand(CommandDescriptor descriptor);
    IReadOnlyList<CommandDescriptor> Commands { get; }
    IEnumerable<CommandDescriptor> Search(string term);
}
public record CommandDescriptor(string Id, string Title, string Category, Action Execute);
public class CommandPaletteService : ICommandPaletteService
{
    private readonly List<CommandDescriptor> _commands = new();
    public IReadOnlyList<CommandDescriptor> Commands => _commands;
    public void RegisterCommand(CommandDescriptor descriptor)
    {
        if (_commands.All(c=>c.Id!=descriptor.Id)) _commands.Add(descriptor);
    }
    public IEnumerable<CommandDescriptor> Search(string term)
    {
        term = term?.Trim() ?? string.Empty;
        return _commands.Where(c=>string.IsNullOrEmpty(term) || c.Title.Contains(term, StringComparison.OrdinalIgnoreCase) || c.Category.Contains(term, StringComparison.OrdinalIgnoreCase));
    }
}

