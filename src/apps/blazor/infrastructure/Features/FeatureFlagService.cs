using Blazored.LocalStorage;

namespace FSH.Starter.Blazor.Infrastructure.Features;

public class FeatureFlagService(ILocalStorageService local) : IFeatureFlagService
{
    private const string StorageKey = "feature-flags";
    private Dictionary<string,bool> _flags = new();
    private bool _initialized;

    public bool IsEnabled(string flag) => _flags.TryGetValue(flag, out var v) && v;
    public IReadOnlyDictionary<string,bool> All() => _flags;

    public async Task SetAsync(string flag, bool enabled)
    {
        await InitializeAsync();
        _flags[flag] = enabled;
        await local.SetItemAsync(StorageKey, _flags).ConfigureAwait(false);
    }

    public async Task InitializeAsync()
    {
        if (_initialized) return;
        _initialized = true;
        var data = await local.GetItemAsync<Dictionary<string,bool>>(StorageKey).ConfigureAwait(false);
        if (data is not null) _flags = data;
        foreach (var f in FeatureFlags.All)
            if (!_flags.ContainsKey(f)) _flags[f] = false;
    }
}

