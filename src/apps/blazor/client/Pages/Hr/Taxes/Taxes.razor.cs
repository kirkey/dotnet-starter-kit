namespace FSH.Starter.Blazor.Client.Pages.Hr.Taxes;

/// <summary>
/// Taxes page for managing tax configurations and rates.
/// Provides CRUD operations for tax master definitions.
/// </summary>
public partial class Taxes
{
    

    // Changed TEntity generic from TaxResponse -> TaxDto to match field definitions and search pagination type.
    protected EntityServerTableContext<TaxDto, DefaultIdType, TaxViewModel> Context { get; set; } = null!;
    
    private ClientPreference _preference = new();

    private EntityTable<TaxDto, DefaultIdType, TaxViewModel>? _table;

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

        Context = new EntityServerTableContext<TaxDto, DefaultIdType, TaxViewModel>(
            entityName: "Tax",
            entityNamePlural: "Taxes",
            entityResource: FshResources.Taxes,
            fields:
            [
                new EntityField<TaxDto>(response => response.Code, "Code", "Code"),
                new EntityField<TaxDto>(response => response.Name, "Name", "Name"),
                new EntityField<TaxDto>(response => response.TaxType, "Type", "TaxType"),
                new EntityField<TaxDto>(response => $"{response.Rate * 100:N2}%", "Rate", "Rate"),
                new EntityField<TaxDto>(response => response.Jurisdiction ?? "-", "Jurisdiction", "Jurisdiction"),
                // Use DateOnly metadata for DateTime field per instruction.
                new EntityField<TaxDto>(response => response.EffectiveDate, "Effective", "EffectiveDate", typeof(DateOnly)),
                new EntityField<TaxDto>(response => response.ExpiryDate, "Expiry", "ExpiryDate", typeof(DateOnly)),
                new EntityField<TaxDto>(response => response.IsCompound, "Compound", "IsCompound", typeof(bool)),
                new EntityField<TaxDto>(response => response.IsActive, "Active", "IsActive", typeof(bool)),
            ],
            enableAdvancedSearch: true,
            idFunc: response => response.Id,
            searchFunc: async filter =>
            {
                var request = new SearchTaxesRequest
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Keyword = filter.Keyword,
                    OrderBy = filter.OrderBy
                };
                var result = await Client.SearchTaxesEndpointAsync("1", request).ConfigureAwait(false);

                return result.Adapt<PaginationResponse<TaxDto>>();
            },
            createFunc: async tax =>
            {
                await Client.CreateTaxEndpointAsync("1", tax.Adapt<CreateTaxCommand>()).ConfigureAwait(false);
            },
            updateFunc: async (id, tax) =>
            {
                await Client.UpdateTaxEndpointAsync("1", id, tax.Adapt<UpdateTaxCommand>()).ConfigureAwait(false);
            },
            deleteFunc: async id =>
            {
                await Client.DeleteTaxEndpointAsync("1", id).ConfigureAwait(false);
            });
    }

    /// <summary>
    /// Shows the taxes help dialog.
    /// </summary>
    private async Task ShowTaxesHelp()
    {
        await DialogService.ShowAsync<TaxesHelpDialog>("Taxes Help", new DialogParameters(), _helpDialogOptions);
    }
}
