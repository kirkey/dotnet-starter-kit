using Blazored.LocalStorage;
namespace FSH.Starter.Blazor.Infrastructure.Localization;
public interface ILocalizationService
{
    string CurrentCulture { get; }
    IReadOnlyList<string> SupportedCultures { get; }
    event Action<string>? CultureChanged;
    Task InitializeAsync();
    Task SetCultureAsync(string culture);
}
public class LocalizationService(ILocalStorageService local) : ILocalizationService
{
    private const string StorageKey = "culture";
    private string _current = "en-US";
    public string CurrentCulture => _current;
    public IReadOnlyList<string> SupportedCultures { get; } = new []{"en-US","es-ES","fr-FR","de-DE","fil-PH"};
    public event Action<string>? CultureChanged;
    private bool _initialized;

    public async Task InitializeAsync()
    {
        if (_initialized) return;
        _initialized = true;
        var saved = await local.GetItemAsync<string>(StorageKey);
        if (!string.IsNullOrWhiteSpace(saved) && SupportedCultures.Contains(saved)) _current = saved;
    }
    public async Task SetCultureAsync(string culture)
    {
        await InitializeAsync();
        if (SupportedCultures.Contains(culture) && _current != culture)
        {
            _current = culture;
            await local.SetItemAsync(StorageKey, culture);
            CultureChanged?.Invoke(culture);
        }
    }
}

