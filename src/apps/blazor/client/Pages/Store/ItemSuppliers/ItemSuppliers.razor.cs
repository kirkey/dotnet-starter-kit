namespace FSH.Starter.Blazor.Client.Pages.Store.ItemSuppliers;

public partial class ItemSuppliers
{
    

    private EntityServerTableContext<ItemSupplierResponse, DefaultIdType, ItemSupplierViewModel> Context { get; set; } =
        null!;

    private EntityTable<ItemSupplierResponse, DefaultIdType, ItemSupplierViewModel> _table = null!;

    private ClientPreference _preference = new();

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

        Context = new EntityServerTableContext<ItemSupplierResponse, DefaultIdType, ItemSupplierViewModel>(
            entityName: "Item Supplier",
            entityNamePlural: "Item Suppliers",
            entityResource: FshResources.Store,
            fields:
            [
                new EntityField<ItemSupplierResponse>(x => x.Name, "Name", "Name"),
                new EntityField<ItemSupplierResponse>(x => x.ItemName, "Item", "ItemName"),
                new EntityField<ItemSupplierResponse>(x => x.SupplierName, "Supplier", "SupplierName"),
                new EntityField<ItemSupplierResponse>(x => x.UnitCost, "Unit Cost", "UnitCost", typeof(decimal)),
                new EntityField<ItemSupplierResponse>(x => x.LeadTimeDays, "Lead Time", "LeadTimeDays", typeof(int)),
                new EntityField<ItemSupplierResponse>(x => x.IsPreferred, "Preferred", "IsPreferred", typeof(bool)),
                new EntityField<ItemSupplierResponse>(x => x.IsActive, "Active", "IsActive", typeof(bool)),
                new EntityField<ItemSupplierResponse>(x => x.ReliabilityRating, "Rating", "ReliabilityRating",
                    typeof(decimal))
            ],
            enableAdvancedSearch: true,
            idFunc: response => response.Id,
            // getDetailsFunc: async id =>
            // {
            //     var dto = await Client.GetItemSupplierEndpointAsync("1", id).ConfigureAwait(false);
            //     return dto.Adapt<ItemSupplierViewModel>();
            // }
            searchFunc: async filter =>
            {
                var paginationFilter = filter.Adapt<PaginationFilter>();
                var command = paginationFilter.Adapt<SearchItemSuppliersCommand>();
                var result = await Client.SearchItemSuppliersEndpointAsync("1", command).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<ItemSupplierResponse>>();
            },
            createFunc: async viewModel =>
            {
                await Client.CreateItemSupplierEndpointAsync("1", viewModel.Adapt<CreateItemSupplierCommand>())
                    .ConfigureAwait(false);
            },
            updateFunc: async (id, viewModel) =>
            {
                await Client.UpdateItemSupplierEndpointAsync("1", id, viewModel.Adapt<UpdateItemSupplierCommand>())
                    .ConfigureAwait(false);
            },
            deleteFunc: async id => await Client.DeleteItemSupplierEndpointAsync("1", id).ConfigureAwait(false));
    }

    /// <summary>
    /// Show item suppliers help dialog.
    /// </summary>
    private async Task ShowItemSuppliersHelp()
    {
        await DialogService.ShowAsync<ItemSuppliersHelpDialog>("Item Suppliers Help", new DialogParameters(), new DialogOptions
        {
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
            CloseOnEscapeKey = true
        });
    }
}

/// <summary>
/// ViewModel for Item Supplier add/edit operations.
/// Inherits from UpdateItemSupplierCommand to ensure proper mapping with the API.
/// </summary>
public partial class ItemSupplierViewModel : UpdateItemSupplierCommand;
