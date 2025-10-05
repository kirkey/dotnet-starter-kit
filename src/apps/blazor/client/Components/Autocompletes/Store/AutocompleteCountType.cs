namespace FSH.Starter.Blazor.Client.Components.Autocompletes.Store;

/// <summary>
/// Autocomplete component for selecting a Cycle Count Type.
/// Provides standard count type values: Full, Partial, ABC, Random, Spot, Zone.
/// </summary>
public class AutocompleteCountType : MudAutocomplete<string>
{
    private static readonly string[] StandardCountTypes = 
    [
        "Full",
        "Partial", 
        "ABC",
        "Random",
        "Spot",
        "Zone"
    ];

    public override Task SetParametersAsync(ParameterView parameters)
    {
        CoerceText = true;
        CoerceValue = true;
        Clearable = true;
        Dense = true;
        ResetValueOnEmptyText = true;
        SearchFunc = SearchText;
        Variant = Variant.Filled;
        return base.SetParametersAsync(parameters);
    }

    private Task<IEnumerable<string>> SearchText(string? value, CancellationToken token)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Task.FromResult<IEnumerable<string>>(StandardCountTypes);
        }

        var filtered = StandardCountTypes
            .Where(x => x.Contains(value, StringComparison.OrdinalIgnoreCase))
            .AsEnumerable();

        return Task.FromResult(filtered);
    }
}

