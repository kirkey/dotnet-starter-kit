namespace FSH.Starter.Blazor.Client.Pages.Store.PickLists;

/// <summary>
/// Dialog component for adding items to a pick list.
/// Allows selection of item, bin location, quantity, and sequence number.
/// </summary>
public partial class AddPickListItemDialog
{
    [Parameter] public DefaultIdType PickListId { get; set; }
    [Parameter] public string PickListNumber { get; set; } = string.Empty;
    
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = default!;

    private bool _loading = true;
    private bool _saving;
    private string? _errorMessage;

    private ItemResponse? _selectedItem;
    private BinResponse? _selectedBin;
    private int _quantityToPick = 1;
    private string? _notes;

    protected override async Task OnInitializedAsync()
    {
        _loading = false;
        await Task.CompletedTask;
    }

    /// <summary>
    /// Searches for items based on the search string.
    /// </summary>
    private async Task<IEnumerable<ItemResponse>> SearchItems(string searchString, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(searchString) || searchString.Length < 2)
        {
            return Enumerable.Empty<ItemResponse>();
        }

        try
        {
            var command = new SearchItemsCommand
            {
                PageNumber = 1,
                PageSize = 20,
                Keyword = searchString,
                OrderBy = ["Name"]
            };

            var result = await Client.SearchItemsEndpointAsync("1", command, cancellationToken).ConfigureAwait(false);
            return result.Items ?? Enumerable.Empty<ItemResponse>();
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Failed to search items: {ex.Message}", Severity.Error);
            return Enumerable.Empty<ItemResponse>();
        }
    }

    /// <summary>
    /// Searches for bins based on the search string.
    /// </summary>
    private async Task<IEnumerable<BinResponse>> SearchBins(string searchString, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(searchString) || searchString.Length < 2)
        {
            return Enumerable.Empty<BinResponse>();
        }

        try
        {
            var command = new SearchBinsCommand
            {
                PageNumber = 1,
                PageSize = 20,
                Keyword = searchString,
                OrderBy = ["Code"]
            };

            var result = await Client.SearchBinsEndpointAsync("1", command, cancellationToken).ConfigureAwait(false);
            return result.Items ?? Enumerable.Empty<BinResponse>();
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Failed to search bins: {ex.Message}", Severity.Error);
            return Enumerable.Empty<BinResponse>();
        }
    }

    /// <summary>
    /// Adds the item to the pick list.
    /// </summary>
    private async Task AddItem()
    {
        if (_selectedItem == null)
        {
            Snackbar.Add("Please select an item", Severity.Warning);
            return;
        }

        if (_quantityToPick <= 0)
        {
            Snackbar.Add("Quantity must be greater than zero", Severity.Warning);
            return;
        }

        try
        {
            _saving = true;
            _errorMessage = null;

            var command = new AddPickListItemCommand
            {
                PickListId = PickListId,
                ItemId = _selectedItem.Id,
                BinId = _selectedBin?.Id,
                QuantityToPick = _quantityToPick,
                Notes = _notes
            };

            await Client.AddPickListItemEndpointAsync("1", PickListId, command).ConfigureAwait(false);

            Snackbar.Add("Item added to pick list successfully", Severity.Success);
            MudDialog.Close(DialogResult.Ok(true));
        }
        catch (Exception ex)
        {
            _errorMessage = $"Failed to add item: {ex.Message}";
            Snackbar.Add(_errorMessage, Severity.Error);
        }
        finally
        {
            _saving = false;
        }
    }

    /// <summary>
    /// Closes the dialog without adding an item.
    /// </summary>
    private void Cancel()
    {
        MudDialog.Cancel();
    }
}

