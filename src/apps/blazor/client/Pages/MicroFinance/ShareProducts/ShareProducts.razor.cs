namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.ShareProducts;

/// <summary>
/// ShareProducts page logic. Provides CRUD and search over ShareProduct entities using the generated API client.
/// Manages share product configurations for member ownership and dividends.
/// </summary>
public partial class ShareProducts
{
    /// <summary>
    /// Table context that drives the generic <see cref="EntityTable{TEntity, TId, TRequest}"/> used in the Razor view.
    /// </summary>
    protected EntityServerTableContext<ShareProductResponse, DefaultIdType, ShareProductViewModel> Context { get; set; } = null!;

    private EntityTable<ShareProductResponse, DefaultIdType, ShareProductViewModel> _table = null!;

    /// <summary>
    /// Client UI preferences for styling.
    /// </summary>
    private ClientPreference _preference = new();

    /// <summary>
    /// Initializes the table context with share product-specific configuration.
    /// </summary>
    protected override async Task OnInitializedAsync()
    {
        // Load initial preference from localStorage
        if (await ClientPreferences.GetPreference() is ClientPreference preference)
        {
            _preference = preference;
        }

        Courier.SubscribeWeak<NotificationWrapper<ClientPreference>>(wrapper =>
        {
            _preference.Elevation = ClientPreference.SetClientPreference(wrapper.Notification);
            _preference.BorderRadius = ClientPreference.SetClientBorderRadius(wrapper.Notification);
            StateHasChanged();
            return Task.CompletedTask;
        });

        Context = new EntityServerTableContext<ShareProductResponse, DefaultIdType, ShareProductViewModel>(
            fields:
            [
                new EntityField<ShareProductResponse>(dto => dto.Code, "Code", "Code"),
                new EntityField<ShareProductResponse>(dto => dto.Name, "Name", "Name"),
                new EntityField<ShareProductResponse>(dto => dto.NominalValue, "Nominal Value", "NominalValue", typeof(decimal)),
                new EntityField<ShareProductResponse>(dto => dto.CurrentPrice, "Current Price", "CurrentPrice", typeof(decimal)),
                new EntityField<ShareProductResponse>(dto => dto.MinSharesForMembership, "Min Shares", "MinSharesForMembership", typeof(int)),
                new EntityField<ShareProductResponse>(dto => dto.MaxSharesPerMember, "Max Shares", "MaxSharesPerMember", typeof(int?)),
                new EntityField<ShareProductResponse>(dto => dto.PaysDividends, "Pays Dividends", "PaysDividends", typeof(bool)),
                new EntityField<ShareProductResponse>(dto => dto.IsActive, "Active", "IsActive", typeof(bool)),
            ],
            searchFunc: async filter =>
            {
                var request = new SearchShareProductsCommand
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Keyword = filter.Keyword,
                    OrderBy = filter.OrderBy
                };
                var result = await Client.SearchShareProductsAsync("1", request).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<ShareProductResponse>>();
            },
            idFunc: dto => dto.Id,
            createFunc: async viewModel =>
            {
                await Client.CreateShareProductAsync("1", viewModel.Adapt<CreateShareProductCommand>()).ConfigureAwait(false);
            },
            updateFunc: async (id, viewModel) =>
            {
                await Client.UpdateShareProductAsync("1", id, viewModel.Adapt<UpdateShareProductCommand>()).ConfigureAwait(false);
            },
            // No delete for share products - they should be deactivated instead
            entityName: "Share Product",
            entityNamePlural: "Share Products",
            entityResource: FshResources.MicroFinance,
            hasExtraActionsFunc: () => true);
    }

    /// <summary>
    /// Show share products help dialog.
    /// </summary>
    private async Task ShowShareProductsHelp()
    {
        await DialogService.ShowAsync<ShareProductsHelpDialog>("Share Products Help", new DialogParameters(), new DialogOptions
        {
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
            CloseOnEscapeKey = true
        });
    }

    /// <summary>
    /// View share product details in a dialog.
    /// </summary>
    private async Task ViewProductDetails(DefaultIdType id)
    {
        var parameters = new DialogParameters
        {
            { nameof(ShareProductDetailsDialog.ProductId), id }
        };

        await DialogService.ShowAsync<ShareProductDetailsDialog>("Share Product Details", parameters, new DialogOptions
        {
            MaxWidth = MaxWidth.Medium,
            FullWidth = true,
            CloseOnEscapeKey = true
        });
    }
}
