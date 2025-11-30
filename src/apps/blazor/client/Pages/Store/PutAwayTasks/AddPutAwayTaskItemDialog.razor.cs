namespace FSH.Starter.Blazor.Client.Pages.Store.PutAwayTasks;

/// <summary>
/// Dialog component for adding items to a put-away task.
/// Allows selecting an item, destination bin, quantity, and sequence for put-away operations.
/// </summary>
public partial class AddPutAwayTaskItemDialog
{
    [Parameter] public DefaultIdType PutAwayTaskId { get; set; }
    [Parameter] public string TaskNumber { get; set; } = string.Empty;
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = null!;

    private ItemResponse? _selectedItem;
    private BinResponse? _selectedToBin;
    private int _quantityToPutAway = 1;
    private int _sequenceNumber = 1;
    private string _notes = string.Empty;
    private bool _loading;
    private bool _adding;
    private string? _errorMessage;
    private ClientPreference _preference = new();

    protected override async Task OnInitializedAsync()
    {
        if (await ClientPreferences.GetPreference() is ClientPreference preference)
        {
            _preference = preference;
        }
    }
    /// <summary>
    /// Searches for items based on the provided search value.
    /// </summary>
    private async Task<IEnumerable<ItemResponse>> SearchItems(string searchValue, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(searchValue) || searchValue.Length < 2)
            return [];

        try
        {
            var command = new SearchItemsCommand
            {
                PageNumber = 1,
                PageSize = 20,
                Keyword = searchValue,
                OrderBy = ["Name"]
            };

            var result = await Client.SearchItemsEndpointAsync("1", command, cancellationToken).ConfigureAwait(false);
            return result.Items ?? Array.Empty<ItemResponse>();
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Failed to search items: {ex.Message}", Severity.Error);
            return [];
        }
    }

    /// <summary>
    /// Searches for bins based on the provided search value.
    /// </summary>
    private async Task<IEnumerable<BinResponse>> SearchBins(string searchValue, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(searchValue) || searchValue.Length < 2)
            return [];

        try
        {
            var command = new SearchBinsCommand
            {
                PageNumber = 1,
                PageSize = 20,
                Keyword = searchValue,
                OrderBy = ["Code"]
            };

            var result = await Client.SearchBinsEndpointAsync("1", command, cancellationToken).ConfigureAwait(false);
            return result.Items ?? Array.Empty<BinResponse>();
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Failed to search bins: {ex.Message}", Severity.Error);
            return [];
        }
    }

    /// <summary>
    /// Adds the selected item to the put-away task by calling the API.
    /// </summary>
    private async Task AddItem()
    {
        if (_selectedItem == null)
        {
            Snackbar.Add("Please select an item", Severity.Warning);
            return;
        }

        if (_selectedToBin == null)
        {
            Snackbar.Add("Please select a destination bin", Severity.Warning);
            return;
        }

        if (_quantityToPutAway <= 0)
        {
            Snackbar.Add("Quantity must be greater than zero", Severity.Warning);
            return;
        }

        if (_sequenceNumber <= 0)
        {
            Snackbar.Add("Sequence number must be greater than zero", Severity.Warning);
            return;
        }

        try
        {
            _adding = true;
            _errorMessage = null;

            var command = new AddPutAwayTaskItemCommand
            {
                PutAwayTaskId = PutAwayTaskId,
                ItemId = _selectedItem.Id,
                ToBinId = _selectedToBin.Id,
                QuantityToPutAway = _quantityToPutAway,
                SequenceNumber = _sequenceNumber,
                Notes = string.IsNullOrWhiteSpace(_notes) ? null : _notes
            };

            await Client.AddPutAwayTaskItemEndpointAsync("1", PutAwayTaskId, command).ConfigureAwait(false);
            
            Snackbar.Add("Item added to put-away task successfully", Severity.Success);
            MudDialog.Close(DialogResult.Ok(true));
        }
        catch (Exception ex)
        {
            _errorMessage = $"Failed to add item: {ex.Message}";
            Snackbar.Add(_errorMessage, Severity.Error);
        }
        finally
        {
            _adding = false;
        }
    }

    /// <summary>
    /// Closes the dialog without adding the item.
    /// </summary>
    private void Cancel()
    {
        MudDialog.Cancel();
    }
}

