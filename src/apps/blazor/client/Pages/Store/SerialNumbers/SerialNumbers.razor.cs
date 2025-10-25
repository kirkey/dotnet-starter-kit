namespace FSH.Starter.Blazor.Client.Pages.Store;

public partial class SerialNumbers
{
    

    private EntityServerTableContext<SerialNumberResponse, DefaultIdType, SerialNumberViewModel> Context { get; set; } = default!;
    private EntityTable<SerialNumberResponse, DefaultIdType, SerialNumberViewModel> _table = default!;

    protected override async Task OnInitializedAsync()
    {
        Context = new EntityServerTableContext<SerialNumberResponse, DefaultIdType, SerialNumberViewModel>(
            entityName: "Serial Number",
            entityNamePlural: "Serial Numbers",
            entityResource: FshResources.Store,
            fields:
            [
                new EntityField<SerialNumberResponse>(x => x.SerialNumberValue, "Serial Number", "SerialNumberValue"),
                new EntityField<SerialNumberResponse>(x => x.ItemId, "Item", "ItemId"),
                new EntityField<SerialNumberResponse>(x => x.Status, "Status", "Status"),
                new EntityField<SerialNumberResponse>(x => x.ReceiptDate, "Receipt Date", "ReceiptDate", typeof(DateTime?)),
                new EntityField<SerialNumberResponse>(x => x.WarrantyExpirationDate, "Warranty Exp", "WarrantyExpirationDate", typeof(DateTime?)),
                new EntityField<SerialNumberResponse>(x => x.ExternalReference, "External Ref", "ExternalReference")
            ],
            enableAdvancedSearch: true,
            idFunc: response => response.Id,
            // getDetailsFunc: async id =>
            // {
            //     var dto = await Client.GetSerialNumberEndpointAsync("1", id).ConfigureAwait(false);
            //     return dto.Adapt<SerialNumberViewModel>();
            // }
            searchFunc: async filter =>
            {
                var paginationFilter = filter.Adapt<PaginationFilter>();
                var command = paginationFilter.Adapt<SearchSerialNumbersCommand>();
                var result = await Client.SearchSerialNumbersEndpointAsync("1", command).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<SerialNumberResponse>>();
            },
            createFunc: async viewModel =>
            {
                await Client.CreateSerialNumberEndpointAsync("1", viewModel.Adapt<CreateSerialNumberCommand>()).ConfigureAwait(false);
            },
            updateFunc: async (id, viewModel) =>
            {
                await Client.UpdateSerialNumberEndpointAsync("1", id, viewModel.Adapt<UpdateSerialNumberCommand>()).ConfigureAwait(false);
            },
            deleteFunc: async id => await Client.DeleteSerialNumberEndpointAsync("1", id).ConfigureAwait(false));
        
        await Task.CompletedTask;
    }
}

/// <summary>
/// ViewModel for Serial Number add/edit operations.
/// Inherits from UpdateSerialNumberCommand to ensure proper mapping with the API.
/// </summary>
public partial class SerialNumberViewModel : UpdateSerialNumberCommand;
