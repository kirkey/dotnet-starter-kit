namespace FSH.Starter.Blazor.Client.Components.Autocompletes;

public abstract class AutocompleteBase<TDto, TClient, TKey> : MudAutocomplete<TKey>
    where TDto : class, new()
    where TClient : class
    where TKey : notnull
{
    private readonly Dictionary<TKey, TDto> Dictionary = new();
    [Inject] protected ISnackbar Snackbar { get; set; } = default!;
    [Inject] protected TClient Client { get; set; } = default!;

    public override Task SetParametersAsync(ParameterView parameters)
    {
        CoerceText = CoerceValue = Clearable = Dense = ResetValueOnEmptyText = true;
        SearchFunc = SearchText;
        ToStringFunc = GetText;
        Variant = Variant.Filled;
        return base.SetParametersAsync(parameters);
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender && !EqualityComparer<TKey>.Default.Equals(_value, default) &&
            await GetItem(_value!).ConfigureAwait(false) is { } dto)
        {
            Dictionary[_value!] = dto;
            ForceRender(true);
        }
    }

    protected abstract Task<TDto?> GetItem(TKey id);
    protected abstract Task<IEnumerable<TKey>>? SearchText(string? value, CancellationToken cancellationToken);
    protected abstract string? GetText(TKey? id);
}
