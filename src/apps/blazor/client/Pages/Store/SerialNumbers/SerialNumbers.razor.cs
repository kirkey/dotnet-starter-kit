namespace FSH.Starter.Blazor.Client.Pages.Store.SerialNumbers;

public partial class SerialNumbers
{
    

    private EntityServerTableContext<SerialNumberResponse, DefaultIdType, SerialNumberViewModel> Context { get; set; } = null!;
    private EntityTable<SerialNumberResponse, DefaultIdType, SerialNumberViewModel> _table = null!;

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
                new EntityField<SerialNumberResponse>(x => x.ReceiptDate, "Receipt Date", "ReceiptDate", typeof(DateOnly?)),
                new EntityField<SerialNumberResponse>(x => x.WarrantyExpirationDate, "Warranty Exp", "WarrantyExpirationDate", typeof(DateOnly?)),
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

    /// <summary>
    /// Show serial numbers help dialog.
    /// </summary>
    private async Task ShowSerialNumbersHelp()
    {
        await DialogService.ShowAsync<SerialNumbersHelpDialog>("Serial Numbers Help", new DialogParameters(), new DialogOptions
        {
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
            CloseOnEscapeKey = true
        });
    }
}

/// <summary>
/// ViewModel for Serial Number add/edit operations.
/// Inherits from UpdateSerialNumberCommand to ensure proper mapping with the API.
/// </summary>
public partial class SerialNumberViewModel : UpdateSerialNumberCommand;
