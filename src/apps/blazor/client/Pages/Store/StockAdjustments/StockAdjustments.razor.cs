namespace FSH.Starter.Blazor.Client.Pages.Store.StockAdjustments;

public partial class StockAdjustments
{
    
    

    protected EntityServerTableContext<StockAdjustmentResponse, DefaultIdType, StockAdjustmentViewModel> Context { get; set; } = null!;
    private EntityTable<StockAdjustmentResponse, DefaultIdType, StockAdjustmentViewModel> _table = null!;

    protected override async Task OnInitializedAsync()
    {
        Context = new EntityServerTableContext<StockAdjustmentResponse, DefaultIdType, StockAdjustmentViewModel>(
            entityName: "Stock Adjustment",
            entityNamePlural: "Stock Adjustments",
            entityResource: FshResources.Store,
            fields:
            [
                new EntityField<StockAdjustmentResponse>(x => x.AdjustmentType, "Type", "AdjustmentType"),
                new EntityField<StockAdjustmentResponse>(x => x.QuantityAdjusted, "Quantity", "QuantityAdjusted", typeof(int)),
                new EntityField<StockAdjustmentResponse>(x => x.Reason, "Reason", "Reason"),
                new EntityField<StockAdjustmentResponse>(x => x.AdjustmentDate, "Date", "AdjustmentDate", typeof(DateTime))
            ],
            enableAdvancedSearch: true,
            idFunc: response => response.Id ?? DefaultIdType.Empty,
            // getDetailsFunc: async id =>
            // {
            //     var dto = await Client.GetStockAdjustmentEndpointAsync("1", id).ConfigureAwait(false);
            //     return dto.Adapt<StockAdjustmentViewModel>();
            // },
            searchFunc: async filter =>
            {
                var command = new SearchStockAdjustmentsCommand
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Keyword = filter.Keyword,
                    OrderBy = filter.OrderBy
                };
                var result = await Client.SearchStockAdjustmentsEndpointAsync("1", command).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<StockAdjustmentResponse>>();
            },
            createFunc: async viewModel =>
            {
                await Client.CreateStockAdjustmentEndpointAsync("1", viewModel.Adapt<CreateStockAdjustmentCommand>()).ConfigureAwait(false);
            },
            updateFunc: async (id, viewModel) =>
            {
                await Client.UpdateStockAdjustmentEndpointAsync("1", id, viewModel.Adapt<UpdateStockAdjustmentCommand>()).ConfigureAwait(false);
            },
            deleteFunc: async id => await Client.DeleteStockAdjustmentEndpointAsync("1", id).ConfigureAwait(false));
        await Task.CompletedTask;
    }

    private async Task ApproveAdjustment(DefaultIdType id)
    {
        bool? result = await DialogService.ShowMessageBox(
            "Confirm Approval",
            "Are you sure you want to approve this adjustment?",
            yesText: "Approve",
            cancelText: "Cancel");

        if (result == true)
        {
            var command = new ApproveStockAdjustmentCommand();
            await Client.ApproveStockAdjustmentEndpointAsync("1", id, command);
            await _table.ReloadDataAsync();
        }
    }
}

/// <summary>
/// ViewModel for Stock Adjustment add/edit operations.
/// Inherits from UpdateStockAdjustmentCommand to ensure proper mapping with the API.
/// </summary>
public partial class StockAdjustmentViewModel : UpdateStockAdjustmentCommand;
