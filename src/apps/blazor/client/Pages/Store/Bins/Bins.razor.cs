namespace FSH.Starter.Blazor.Client.Pages.Store.Bins;

/// <summary>
/// Bins page logic. Provides CRUD and search over Bin entities using the generated API client.
/// </summary>
public partial class Bins
{
    

    protected EntityServerTableContext<BinResponse, DefaultIdType, BinViewModel> Context { get; set; } = null!;
    
    private ClientPreference _preference = new();
    private EntityTable<BinResponse, DefaultIdType, BinViewModel> _table = null!;

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

        Context = new EntityServerTableContext<BinResponse, DefaultIdType, BinViewModel>(
            entityName: "Bin",
            entityNamePlural: "Bins",
            entityResource: FshResources.Store,
            fields:
            [
                new EntityField<BinResponse>(x => x.Code, "Code", "Code"),
                new EntityField<BinResponse>(x => x.Name, "Name", "Name"),
                new EntityField<BinResponse>(x => x.LocationType, "Type", "LocationType"),
                new EntityField<BinResponse>(x => x.Capacity, "Capacity", "Capacity", typeof(decimal)),
                new EntityField<BinResponse>(x => x.IsActive, "Active", "IsActive", typeof(bool)),
                new EntityField<BinResponse>(x => x.WarehouseLocationName, "Location", "WarehouseLocationName")
            ],
            enableAdvancedSearch: true,
            idFunc: response => response.Id,
            // getDetailsFunc: async id =>
            // {
            //     var dto = await Client.GetBinEndpointAsync("1", id).ConfigureAwait(false);
            //     return dto.Adapt<BinViewModel>();
            // }
            searchFunc: async filter =>
            {
                var paginationFilter = filter.Adapt<PaginationFilter>();
                var command = paginationFilter.Adapt<SearchBinsCommand>();
                var result = await Client.SearchBinsEndpointAsync("1", command).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<BinResponse>>();
            },
            createFunc: async viewModel =>
            {
                var command = viewModel.Adapt<CreateBinCommand>();
                command.Code = viewModel.Code!.ToUpperInvariant();
                command.Name = viewModel.Name!.ToUpperInvariant();
                await Client.CreateBinEndpointAsync("1", command).ConfigureAwait(false);
            },
            updateFunc: async (id, viewModel) =>
            {
                var command = viewModel.Adapt<UpdateBinCommand>();
                command.Code = viewModel.Code!.ToUpperInvariant();
                command.Name = viewModel.Name!.ToUpperInvariant();
                await Client.UpdateBinEndpointAsync("1", id, command).ConfigureAwait(false);
            },
            deleteFunc: async id => await Client.DeleteBinEndpointAsync("1", id).ConfigureAwait(false));

    await Task.CompletedTask;
    }

    /// <summary>
    /// Show bins help dialog.
    /// </summary>
    private async Task ShowBinsHelp()
    {
        await DialogService.ShowAsync<BinsHelpDialog>("Bins Help", new DialogParameters(), new DialogOptions
        {
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
            CloseOnEscapeKey = true
        });
    }
}

/// <summary>
/// ViewModel for Bin add/edit operations.
/// Inherits from UpdateBinCommand to ensure proper mapping with the API.
/// </summary>
public partial class BinViewModel : UpdateBinCommand;
