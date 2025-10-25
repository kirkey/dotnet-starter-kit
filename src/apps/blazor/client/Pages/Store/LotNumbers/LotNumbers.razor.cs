namespace FSH.Starter.Blazor.Client.Pages.Store.LotNumbers;

public partial class LotNumbers
{
    

    private EntityServerTableContext<LotNumberResponse, DefaultIdType, LotNumberViewModel> Context { get; set; } = default!;
    private EntityTable<LotNumberResponse, DefaultIdType, LotNumberViewModel> _table = default!;

    protected override async Task OnInitializedAsync()
    {
        Context = new EntityServerTableContext<LotNumberResponse, DefaultIdType, LotNumberViewModel>(
            entityName: "Lot Number",
            entityNamePlural: "Lot Numbers",
            entityResource: FshResources.Store,
            fields:
            [
                new EntityField<LotNumberResponse>(x => x.LotCode, "Lot Code", "LotCode"),
                new EntityField<LotNumberResponse>(x => x.ItemId, "Item", "ItemId"),
                new EntityField<LotNumberResponse>(x => x.QuantityReceived, "Qty Received", "QuantityReceived", typeof(int)),
                new EntityField<LotNumberResponse>(x => x.QuantityRemaining, "Qty Remaining", "QuantityRemaining", typeof(int)),
                new EntityField<LotNumberResponse>(x => x.ManufactureDate, "Mfg Date", "ManufactureDate", typeof(DateTime?)),
                new EntityField<LotNumberResponse>(x => x.ExpirationDate, "Exp Date", "ExpirationDate", typeof(DateTime?)),
                new EntityField<LotNumberResponse>(x => x.Status, "Status", "Status")
            ],
            enableAdvancedSearch: true,
            idFunc: response => response.Id,
            // getDetailsFunc: async id =>
            // {
            //     var dto = await Client.GetLotNumberEndpointAsync("1", id).ConfigureAwait(false);
            //     return dto.Adapt<LotNumberViewModel>();
            // }
            searchFunc: async filter =>
            {
                var paginationFilter = filter.Adapt<PaginationFilter>();
                var command = paginationFilter.Adapt<SearchLotNumbersCommand>();
                var result = await Blazor.Client.SearchLotNumbersEndpointAsync("1", command).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<LotNumberResponse>>();
            },
            createFunc: async viewModel =>
            {
                await Blazor.Client.CreateLotNumberEndpointAsync("1", viewModel.Adapt<CreateLotNumberCommand>()).ConfigureAwait(false);
            },
            updateFunc: async (id, viewModel) =>
            {
                await Blazor.Client.UpdateLotNumberEndpointAsync("1", id, viewModel.Adapt<UpdateLotNumberCommand>()).ConfigureAwait(false);
            },
            deleteFunc: async id => await Blazor.Client.DeleteLotNumberEndpointAsync("1", id).ConfigureAwait(false));
        
        await Task.CompletedTask;
    }
}

/// <summary>
/// ViewModel for Lot Number add/edit operations.
/// Inherits from UpdateLotNumberCommand to ensure proper mapping with the API.
/// </summary>
public partial class LotNumberViewModel : UpdateLotNumberCommand;
