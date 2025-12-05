using FSH.Starter.Blazor.Infrastructure.Api;
namespace FSH.Starter.Blazor.Client.Pages.Hr.TaxBrackets;

/// <summary>
/// Tax brackets page for managing income tax brackets.
/// Provides CRUD operations for tax bracket configuration.
/// </summary>
public partial class TaxBrackets
{
    

    protected EntityServerTableContext<TaxBracketResponse, DefaultIdType, TaxBracketViewModel> Context { get; set; } = null!;
    
    private ClientPreference _preference = new();

    private EntityTable<TaxBracketResponse, DefaultIdType, TaxBracketViewModel>? _table;

    private readonly DialogOptions _helpDialogOptions = new() { CloseOnEscapeKey = true, MaxWidth = MaxWidth.Large, FullWidth = true };

    /// <summary>
    /// Initializes the component and sets up the entity table context with CRUD operations.
    /// </summary>
    protected override async Task OnInitializedAsync()
    {
        // Load preference
        if (await ClientPreferences.GetPreference() is ClientPreference preference)
        {
            _preference = preference;
        }

        // Subscribe to preference changes
        Courier.SubscribeWeak<NotificationWrapper<ClientPreference>>(wrapper =>
        {
            _preference.Elevation = ClientPreference.SetClientPreference(wrapper.Notification);
            _preference.BorderRadius = ClientPreference.SetClientBorderRadius(wrapper.Notification);
            StateHasChanged();
            return Task.CompletedTask;
        });

        Context = new EntityServerTableContext<TaxBracketResponse, DefaultIdType, TaxBracketViewModel>(
            entityName: "Tax Bracket",
            entityNamePlural: "Tax Brackets",
            entityResource: FshResources.Taxes,
            fields:
            [
                new EntityField<TaxBracketResponse>(response => response.TaxType ?? "-", "Tax Type", "TaxType"),
                new EntityField<TaxBracketResponse>(response => response.Year, "Year", "Year"),
                new EntityField<TaxBracketResponse>(response => response.MinIncome.ToString("C"), "Min Income", "MinIncome"),
                new EntityField<TaxBracketResponse>(response => response.MaxIncome.ToString("C"), "Max Income", "MaxIncome"),
                new EntityField<TaxBracketResponse>(response => $"{response.Rate * 100:N2}%", "Rate", "Rate"),
                new EntityField<TaxBracketResponse>(response => response.FilingStatus ?? "-", "Filing Status", "FilingStatus"),
            ],
            enableAdvancedSearch: true,
            idFunc: response => response.Id,
            searchFunc: async filter =>
            {
                var request = new SearchTaxBracketsRequest
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Keyword = filter.Keyword,
                    OrderBy = filter.OrderBy
                };
                var result = await Client.SearchTaxBracketsEndpointAsync("1", request).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<TaxBracketResponse>>();
            },
            createFunc: async taxBracket =>
            {
                await Client.CreateTaxBracketAsync("1", taxBracket.Adapt<CreateTaxBracketCommand>()).ConfigureAwait(false);
            },
            updateFunc: async (id, taxBracket) =>
            {
                await Client.UpdateTaxBracketAsync("1", id, taxBracket.Adapt<UpdateTaxBracketCommand>()).ConfigureAwait(false);
            },
            deleteFunc: async id =>
            {
                await Client.DeleteTaxBracketAsync("1", id).ConfigureAwait(false);
            });

        await base.OnInitializedAsync();
    }

    /// <summary>
    /// Shows the tax brackets help dialog.
    /// </summary>
    private async Task ShowTaxBracketsHelp()
    {
        await DialogService.ShowAsync<TaxBracketsHelpDialog>("Tax Brackets Help", new DialogParameters(), _helpDialogOptions);
    }
}
