namespace FSH.Starter.Blazor.Client.Pages.Store.Items;

public partial class AddItemToSupplierDialog
{
    [Parameter]
    public DefaultIdType ItemId { get; set; }

    [Parameter]
    public string ItemName { get; set; } = string.Empty;

    [CascadingParameter]
    private IMudDialogInstance MudDialog { get; set; } = null!;

    private MudForm _form = null!;
    private string _itemName = string.Empty;

    private CreateItemSupplierCommand Model { get; set; } = new()
    {
        UnitCost = 0,
        LeadTimeDays = 7,
        MinimumOrderQuantity = 1,
        IsPreferred = false
    };

    protected override void OnInitialized()
    {
        _itemName = ItemName;
        Model.ItemId = ItemId;
    }

    private void Cancel()
    {
        MudDialog.Cancel();
    }

    private async Task Submit()
    {
        await _form.Validate();

        if (!_form.IsValid)
        {
            return;
        }

        try
        {
            await Client.CreateItemSupplierEndpointAsync("1", Model);
            Snackbar.Add($"Item successfully added to supplier", Severity.Success);
            MudDialog.Close(DialogResult.Ok(true));
        }
        catch (ApiException ex) when (ex.StatusCode == 409)
        {
            // Handle duplicate ItemSupplier error - extract detail from response
            string errorMessage = "This item is already linked to the selected supplier";
            
            // Try to extract the detailed error message from the response
            if (!string.IsNullOrEmpty(ex.Response))
            {
                try
                {
                    using var doc = System.Text.Json.JsonDocument.Parse(ex.Response);
                    if (doc.RootElement.TryGetProperty("detail", out var detailElement))
                    {
                        errorMessage = detailElement.GetString() ?? errorMessage;
                    }
                }
                catch
                {
                    // Fall back to default message if parsing fails
                }
            }
            
            Snackbar.Add(errorMessage, Severity.Error);
        }
        catch (ApiException ex) when (ex.StatusCode == 400)
        {
            // Handle validation errors
            Snackbar.Add($"Validation error: {ex.Message}", Severity.Error);
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error adding item to supplier: {ex.Message}", Severity.Error);
        }
    }
}
