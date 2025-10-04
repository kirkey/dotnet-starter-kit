namespace FSH.Starter.Blazor.Client.Pages.Store;

/// <summary>
/// Bins page logic. Provides CRUD and search over Bin entities using the generated API client.
/// </summary>
public partial class Bins
{
    private EntityServerTableContext<BinResponse, DefaultIdType, BinViewModel> Context = default!;
    private EntityTable<BinResponse, DefaultIdType, BinViewModel> _table = default!;

    protected override async Task OnInitializedAsync()
    {
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
            searchFunc: async filter =>
            {
                var paginationFilter = filter.Adapt<PaginationFilter>();
                var command = paginationFilter.Adapt<SearchBinsCommand>();
                var result = await Client.SearchBinsEndpointAsync("1", command).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<BinResponse>>();
            },
            createFunc: async viewModel =>
            {
                await Client.CreateBinEndpointAsync("1", viewModel.Adapt<CreateBinCommand>()).ConfigureAwait(false);
            },
            updateFunc: async (id, viewModel) =>
            {
                await Client.UpdateBinEndpointAsync("1", id, viewModel.Adapt<UpdateBinCommand>()).ConfigureAwait(false);
            },
            deleteFunc: async id => await Client.DeleteBinEndpointAsync("1", id).ConfigureAwait(false),
            getDetailsFunc: async id =>
            {
                var dto = await Client.GetBinEndpointAsync("1", id).ConfigureAwait(false);
                return dto.Adapt<BinViewModel>();
            });

    await Task.CompletedTask;
    }
}

/// <summary>
/// ViewModel for Bin add/edit operations.
/// Inherits from UpdateBinCommand to ensure proper mapping with the API.
/// </summary>
public partial class BinViewModel : UpdateBinCommand
{
    public string? Code { get; set;}
    public DefaultIdType WarehouseLocationId { get; set;}
    public bool? IsActive { get; set;}
}
